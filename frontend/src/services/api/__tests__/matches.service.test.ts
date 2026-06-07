import { describe, it, expect, vi, beforeEach } from 'vitest';
import { matchesService } from '../matches.service';
import { apiClient } from '../../../config/api';
import { mockMatch, mockMatches, mockPagedMatches } from '../../../test/mockData';
import { MatchPhase, MatchStatus } from '../../../types';

// Mock the API client
vi.mock('../../../config/api', () => ({
  apiClient: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    patch: vi.fn(),
    delete: vi.fn(),
  },
}));

describe('matchesService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('getAll', () => {
    it('fetches all matches with default pagination', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockPagedMatches });

      const result = await matchesService.getAll();

      expect(apiClient.get).toHaveBeenCalledWith('/matches', {
        params: undefined,
      });
      expect(result).toEqual(mockPagedMatches);
    });

    it('fetches all matches with custom pagination', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockPagedMatches });

      await matchesService.getAll({ page: 2, pageSize: 20 });

      expect(apiClient.get).toHaveBeenCalledWith('/matches', {
        params: { page: 2, pageSize: 20 },
      });
    });
  });

  describe('getById', () => {
    it('fetches a match by ID', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockMatch });

      const result = await matchesService.getById(1);

      expect(apiClient.get).toHaveBeenCalledWith('/Matches/1');
      expect(result).toEqual(mockMatch);
    });
  });

  describe('getByPhase', () => {
    it('fetches matches by phase', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockMatches });

      const result = await matchesService.getByPhase(MatchPhase.GroupStage);

      expect(apiClient.get).toHaveBeenCalledWith('/Matches/phase/GroupStage');
      expect(result).toEqual(mockMatches);
    });
  });

  describe('getByStatus', () => {
    it('fetches matches by status', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockMatches });

      const result = await matchesService.getByStatus(MatchStatus.Scheduled);

      expect(apiClient.get).toHaveBeenCalledWith('/Matches/status/Scheduled');
      expect(result).toEqual(mockMatches);
    });
  });

  describe('getByTeam', () => {
    it('fetches matches by team ID', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockMatches });

      const result = await matchesService.getByTeam(1);

      expect(apiClient.get).toHaveBeenCalledWith('/Matches/team/1');
      expect(result).toEqual(mockMatches);
    });
  });

  describe('getByGroup', () => {
    it('fetches matches by group ID', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockMatches });

      const result = await matchesService.getByGroup(1);

      expect(apiClient.get).toHaveBeenCalledWith('/Matches/group/1');
      expect(result).toEqual(mockMatches);
    });
  });

  describe('getByStadium', () => {
    it('fetches matches by stadium ID', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockMatches });

      const result = await matchesService.getByStadium(1);

      expect(apiClient.get).toHaveBeenCalledWith('/Matches/stadium/1');
      expect(result).toEqual(mockMatches);
    });
  });

  describe('getUpcoming', () => {
    it('fetches upcoming matches', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockMatches });

      const result = await matchesService.getUpcoming();

      expect(apiClient.get).toHaveBeenCalledWith('/Matches/upcoming');
      expect(result).toEqual(mockMatches);
    });
  });

  describe('getRecent', () => {
    it('fetches recent matches', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockMatches });

      const result = await matchesService.getRecent();

      expect(apiClient.get).toHaveBeenCalledWith('/Matches/recent');
      expect(result).toEqual(mockMatches);
    });
  });

  describe('create', () => {
    it('creates a new match', async () => {
      const newMatch = {
        homeTeamId: 1,
        awayTeamId: 2,
        stadiumId: 1,
        matchDate: '2026-06-15T20:00:00Z',
        phase: MatchPhase.GroupStage,
        round: 1,
        groupId: 1,
      };
      
      vi.mocked(apiClient.post).mockResolvedValue({ data: mockMatch });

      const result = await matchesService.create(newMatch);

      expect(apiClient.post).toHaveBeenCalledWith('/Matches', newMatch);
      expect(result).toEqual(mockMatch);
    });
  });

  describe('update', () => {
    it('updates an existing match', async () => {
      const updates = {
        homeTeamId: 1,
        awayTeamId: 2,
        stadiumId: 1,
        matchDate: '2026-06-15T20:00:00Z',
        phase: MatchPhase.GroupStage,
        status: MatchStatus.Completed,
      };
      vi.mocked(apiClient.put).mockResolvedValue({ data: { ...mockMatch, ...updates } });

      const result = await matchesService.update(1, updates);

      expect(apiClient.put).toHaveBeenCalledWith('/Matches/1', updates);
      expect(result.status).toBe(MatchStatus.Completed);
    });
  });

  describe('updateResult', () => {
    it('updates match result', async () => {
      const resultUpdate = {
        homeTeamScore: 2,
        awayTeamScore: 1,
      };
      vi.mocked(apiClient.patch).mockResolvedValue({ data: mockMatch });

      const result = await matchesService.updateResult(1, resultUpdate);

      expect(apiClient.patch).toHaveBeenCalledWith('/Matches/1/result', resultUpdate);
      expect(result).toEqual(mockMatch);
    });
  });

  describe('delete', () => {
    it('deletes a match', async () => {
      vi.mocked(apiClient.delete).mockResolvedValue({ data: undefined });

      await matchesService.delete(1);

      expect(apiClient.delete).toHaveBeenCalledWith('/Matches/1');
    });
  });
});

// Made with Bob