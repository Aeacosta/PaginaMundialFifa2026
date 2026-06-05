import api from '../../config/api';
import type { MatchDto, CreateMatchDto, UpdateMatchDto, UpdateMatchResultDto, PagedResult } from '../../types';

export const matchesService = {
  /**
   * Get all matches with pagination and filtering
   */
  getAll: async (params?: {
    page?: number;
    pageSize?: number;
    groupId?: number;
    stadiumId?: number;
    phase?: string;
    status?: string;
    teamId?: number;
  }): Promise<PagedResult<MatchDto>> => {
    const response = await api.get<PagedResult<MatchDto>>('/matches', { params });
    return response.data;
  },

  /**
   * Get match by ID
   */
  getById: async (id: number): Promise<MatchDto> => {
    const response = await api.get<MatchDto>(`/matches/${id}`);
    return response.data;
  },

  /**
   * Get matches by group
   */
  getByGroup: async (groupId: number): Promise<MatchDto[]> => {
    const response = await api.get<MatchDto[]>(`/matches/group/${groupId}`);
    return response.data;
  },

  /**
   * Get matches by stadium
   */
  getByStadium: async (stadiumId: number): Promise<MatchDto[]> => {
    const response = await api.get<MatchDto[]>(`/matches/stadium/${stadiumId}`);
    return response.data;
  },

  /**
   * Get matches by team
   */
  getByTeam: async (teamId: number): Promise<MatchDto[]> => {
    const response = await api.get<MatchDto[]>(`/matches/team/${teamId}`);
    return response.data;
  },

  /**
   * Get matches by phase
   */
  getByPhase: async (phase: string): Promise<MatchDto[]> => {
    const response = await api.get<MatchDto[]>(`/matches/phase/${phase}`);
    return response.data;
  },

  /**
   * Get matches by status
   */
  getByStatus: async (status: string): Promise<MatchDto[]> => {
    const response = await api.get<MatchDto[]>(`/matches/status/${status}`);
    return response.data;
  },

  /**
   * Get upcoming matches
   */
  getUpcoming: async (count: number = 10): Promise<MatchDto[]> => {
    const response = await api.get<MatchDto[]>('/matches/upcoming', {
      params: { count },
    });
    return response.data;
  },

  /**
   * Get recent matches
   */
  getRecent: async (count: number = 10): Promise<MatchDto[]> => {
    const response = await api.get<MatchDto[]>('/matches/recent', {
      params: { count },
    });
    return response.data;
  },

  /**
   * Create a new match
   */
  create: async (data: CreateMatchDto): Promise<MatchDto> => {
    const response = await api.post<MatchDto>('/matches', data);
    return response.data;
  },

  /**
   * Update an existing match
   */
  update: async (id: number, data: UpdateMatchDto): Promise<MatchDto> => {
    const response = await api.put<MatchDto>(`/matches/${id}`, data);
    return response.data;
  },

  /**
   * Update match result
   */
  updateResult: async (id: number, data: UpdateMatchResultDto): Promise<MatchDto> => {
    const response = await api.put<MatchDto>(`/matches/${id}/result`, data);
    return response.data;
  },

  /**
   * Delete a match
   */
  delete: async (id: number): Promise<void> => {
    await api.delete(`/matches/${id}`);
  },

  /**
   * Check if match exists
   */
  exists: async (id: number): Promise<boolean> => {
    try {
      await api.head(`/matches/${id}`);
      return true;
    } catch {
      return false;
    }
  },
};

// Made with Bob
