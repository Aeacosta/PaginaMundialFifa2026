import { describe, it, expect, vi, beforeEach } from 'vitest';
import { teamsService } from '../teams.service';
import { apiClient } from '../../../config/api';
import { mockTeam, mockTeams, mockPagedTeams } from '../../../test/mockData';

// Mock the API client
vi.mock('../../../config/api', () => ({
  apiClient: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    delete: vi.fn(),
  },
}));

describe('teamsService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('getAll', () => {
    it('fetches all teams with default pagination', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockPagedTeams });

      const result = await teamsService.getAll();

      expect(apiClient.get).toHaveBeenCalledWith('/Teams', {
        params: { pageNumber: 1, pageSize: 10 },
      });
      expect(result).toEqual(mockPagedTeams);
    });

    it('fetches all teams with custom pagination', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockPagedTeams });

      await teamsService.getAll(2, 20);

      expect(apiClient.get).toHaveBeenCalledWith('/Teams', {
        params: { pageNumber: 2, pageSize: 20 },
      });
    });
  });

  describe('getById', () => {
    it('fetches a team by ID', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockTeam });

      const result = await teamsService.getById(1);

      expect(apiClient.get).toHaveBeenCalledWith('/Teams/1');
      expect(result).toEqual(mockTeam);
    });
  });

  describe('getByCode', () => {
    it('fetches a team by code', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockTeam });

      const result = await teamsService.getByCode('BRA');

      expect(apiClient.get).toHaveBeenCalledWith('/Teams/code/BRA');
      expect(result).toEqual(mockTeam);
    });
  });

  describe('getByGroup', () => {
    it('fetches teams by group ID', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockTeams });

      const result = await teamsService.getByGroup(1);

      expect(apiClient.get).toHaveBeenCalledWith('/Teams/group/1');
      expect(result).toEqual(mockTeams);
    });
  });

  describe('getByConfederation', () => {
    it('fetches teams by confederation', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockTeams });

      const result = await teamsService.getByConfederation('CONMEBOL');

      expect(apiClient.get).toHaveBeenCalledWith('/Teams/confederation/CONMEBOL');
      expect(result).toEqual(mockTeams);
    });
  });

  describe('search', () => {
    it('searches teams by search term', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockTeams });

      const result = await teamsService.search('Brazil');

      expect(apiClient.get).toHaveBeenCalledWith('/Teams/search', {
        params: { searchTerm: 'Brazil' },
      });
      expect(result).toEqual(mockTeams);
    });
  });

  describe('create', () => {
    it('creates a new team', async () => {
      const newTeam = { ...mockTeam };
      delete (newTeam as any).id;
      
      vi.mocked(apiClient.post).mockResolvedValue({ data: mockTeam });

      const result = await teamsService.create(newTeam);

      expect(apiClient.post).toHaveBeenCalledWith('/Teams', newTeam);
      expect(result).toEqual(mockTeam);
    });
  });

  describe('update', () => {
    it('updates an existing team', async () => {
      const updates = { name: 'Updated Brazil' };
      vi.mocked(apiClient.put).mockResolvedValue({ data: { ...mockTeam, ...updates } });

      const result = await teamsService.update(1, updates);

      expect(apiClient.put).toHaveBeenCalledWith('/Teams/1', updates);
      expect(result.name).toBe('Updated Brazil');
    });
  });

  describe('delete', () => {
    it('deletes a team', async () => {
      vi.mocked(apiClient.delete).mockResolvedValue({ data: undefined });

      await teamsService.delete(1);

      expect(apiClient.delete).toHaveBeenCalledWith('/Teams/1');
    });
  });
});

// Made with Bob