import { apiClient } from '../../config/api';
import type { TeamDto, PagedResult } from '../../types';

export const teamsService = {
  // Get all teams with pagination
  getAll: async (pageNumber = 1, pageSize = 10) => {
    const response = await apiClient.get<PagedResult<TeamDto>>('/Teams', {
      params: { pageNumber, pageSize },
    });
    return response.data;
  },

  // Get team by ID
  getById: async (id: number) => {
    const response = await apiClient.get<TeamDto>(`/Teams/${id}`);
    return response.data;
  },

  // Get team by code
  getByCode: async (code: string) => {
    const response = await apiClient.get<TeamDto>(`/Teams/code/${code}`);
    return response.data;
  },

  // Get teams by group
  getByGroup: async (groupId: number) => {
    const response = await apiClient.get<TeamDto[]>(`/Teams/group/${groupId}`);
    return response.data;
  },

  // Get teams by confederation
  getByConfederation: async (confederation: string) => {
    const response = await apiClient.get<TeamDto[]>(`/Teams/confederation/${confederation}`);
    return response.data;
  },

  // Search teams
  search: async (searchTerm: string) => {
    const response = await apiClient.get<TeamDto[]>('/Teams/search', {
      params: { searchTerm },
    });
    return response.data;
  },

  // Create team
  create: async (team: Omit<TeamDto, 'id'>) => {
    const response = await apiClient.post<TeamDto>('/Teams', team);
    return response.data;
  },

  // Update team
  update: async (id: number, team: Partial<TeamDto>) => {
    const response = await apiClient.put<TeamDto>(`/Teams/${id}`, team);
    return response.data;
  },

  // Delete team
  delete: async (id: number) => {
    await apiClient.delete(`/Teams/${id}`);
  },
};

// Made with Bob
