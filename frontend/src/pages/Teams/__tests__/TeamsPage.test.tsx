import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '../../../test/utils';
import TeamsPage from '../TeamsPage';
import { teamsService } from '../../../services/api/teams.service';
import { mockPagedTeams } from '../../../test/mockData';

// Mock the teams service
vi.mock('../../../services/api/teams.service', () => ({
  teamsService: {
    getAll: vi.fn(),
  },
}));

describe('TeamsPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders loading state initially', () => {
    vi.mocked(teamsService.getAll).mockImplementation(
      () => new Promise(() => {}) // Never resolves
    );

    render(<TeamsPage />);

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('renders teams list when data is loaded', async () => {
    vi.mocked(teamsService.getAll).mockResolvedValue(mockPagedTeams);

    render(<TeamsPage />);

    await waitFor(() => {
      expect(screen.getByText('Brazil')).toBeInTheDocument();
    });

    expect(screen.getByText('Argentina')).toBeInTheDocument();
    expect(screen.getByText('Germany')).toBeInTheDocument();
  });

  it('renders error message when fetch fails', async () => {
    vi.mocked(teamsService.getAll).mockRejectedValue(new Error('Failed to fetch'));

    render(<TeamsPage />);

    await waitFor(() => {
      expect(screen.getByText(/error/i)).toBeInTheDocument();
    });
  });

  it('displays team codes', async () => {
    vi.mocked(teamsService.getAll).mockResolvedValue(mockPagedTeams);

    render(<TeamsPage />);

    await waitFor(() => {
      expect(screen.getByText('BRA')).toBeInTheDocument();
    });

    expect(screen.getByText('ARG')).toBeInTheDocument();
    expect(screen.getByText('GER')).toBeInTheDocument();
  });

  it('displays group information', async () => {
    vi.mocked(teamsService.getAll).mockResolvedValue(mockPagedTeams);

    render(<TeamsPage />);

    await waitFor(() => {
      expect(screen.getByText('Group A')).toBeInTheDocument();
    });

    expect(screen.getByText('Group B')).toBeInTheDocument();
  });
});

// Made with Bob