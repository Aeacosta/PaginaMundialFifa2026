import api from '../../config/api';
import type { StandingDto } from '../../types';

export const standingsService = {
  /**
   * Get all standings
   */
  getAll: async (): Promise<StandingDto[]> => {
    const response = await api.get<StandingDto[]>('/standings');
    return response.data;
  },

  /**
   * Get standings by group
   */
  getByGroup: async (groupId: number): Promise<StandingDto[]> => {
    const response = await api.get<StandingDto[]>(`/standings/group/${groupId}`);
    return response.data;
  },

  /**
   * Get standing by team
   */
  getByTeam: async (teamId: number): Promise<StandingDto> => {
    const response = await api.get<StandingDto>(`/standings/team/${teamId}`);
    return response.data;
  },

  /**
   * Recalculate all standings
   */
  recalculate: async (): Promise<void> => {
    await api.post('/standings/recalculate');
  },

  /**
   * Recalculate standings for a specific group
   */
  recalculateGroup: async (groupId: number): Promise<void> => {
    await api.post(`/standings/recalculate/group/${groupId}`);
  },
};

// Made with Bob
