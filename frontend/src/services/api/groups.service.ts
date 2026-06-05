import api from '../../config/api';
import type { GroupDto, CreateGroupDto, UpdateGroupDto, GroupWithStandingsDto } from '../../types';

export const groupsService = {
  /**
   * Get all groups
   */
  getAll: async (): Promise<GroupDto[]> => {
    const response = await api.get<GroupDto[]>('/groups');
    return response.data;
  },

  /**
   * Get group by ID
   */
  getById: async (id: number): Promise<GroupDto> => {
    const response = await api.get<GroupDto>(`/groups/${id}`);
    return response.data;
  },

  /**
   * Get group by name
   */
  getByName: async (name: string): Promise<GroupDto> => {
    const response = await api.get<GroupDto>(`/groups/name/${name}`);
    return response.data;
  },

  /**
   * Get group with standings
   */
  getWithStandings: async (id: number): Promise<GroupWithStandingsDto> => {
    const response = await api.get<GroupWithStandingsDto>(`/groups/${id}/standings`);
    return response.data;
  },

  /**
   * Get all groups with standings
   */
  getAllWithStandings: async (): Promise<GroupWithStandingsDto[]> => {
    const response = await api.get<GroupWithStandingsDto[]>('/groups/with-standings');
    return response.data;
  },

  /**
   * Create a new group
   */
  create: async (data: CreateGroupDto): Promise<GroupDto> => {
    const response = await api.post<GroupDto>('/groups', data);
    return response.data;
  },

  /**
   * Update an existing group
   */
  update: async (id: number, data: UpdateGroupDto): Promise<GroupDto> => {
    const response = await api.put<GroupDto>(`/groups/${id}`, data);
    return response.data;
  },

  /**
   * Delete a group
   */
  delete: async (id: number): Promise<void> => {
    await api.delete(`/groups/${id}`);
  },

  /**
   * Check if group exists
   */
  exists: async (id: number): Promise<boolean> => {
    try {
      await api.head(`/groups/${id}`);
      return true;
    } catch {
      return false;
    }
  },
};

// Made with Bob
