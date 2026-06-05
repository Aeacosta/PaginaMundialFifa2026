import api from '../../config/api';
import type { StadiumDto, CreateStadiumDto, UpdateStadiumDto, PagedResult } from '../../types';

export const stadiumsService = {
  /**
   * Get all stadiums with pagination and filtering
   */
  getAll: async (params?: {
    page?: number;
    pageSize?: number;
    country?: string;
    city?: string;
    search?: string;
  }): Promise<PagedResult<StadiumDto>> => {
    const response = await api.get<PagedResult<StadiumDto>>('/stadiums', { params });
    return response.data;
  },

  /**
   * Get stadium by ID
   */
  getById: async (id: number): Promise<StadiumDto> => {
    const response = await api.get<StadiumDto>(`/stadiums/${id}`);
    return response.data;
  },

  /**
   * Get stadiums by country
   */
  getByCountry: async (country: string): Promise<StadiumDto[]> => {
    const response = await api.get<StadiumDto[]>(`/stadiums/country/${country}`);
    return response.data;
  },

  /**
   * Get stadiums by city
   */
  getByCity: async (city: string): Promise<StadiumDto[]> => {
    const response = await api.get<StadiumDto[]>(`/stadiums/city/${city}`);
    return response.data;
  },

  /**
   * Search stadiums
   */
  search: async (searchTerm: string): Promise<StadiumDto[]> => {
    const response = await api.get<StadiumDto[]>('/stadiums/search', {
      params: { searchTerm },
    });
    return response.data;
  },

  /**
   * Create a new stadium
   */
  create: async (data: CreateStadiumDto): Promise<StadiumDto> => {
    const response = await api.post<StadiumDto>('/stadiums', data);
    return response.data;
  },

  /**
   * Update an existing stadium
   */
  update: async (id: number, data: UpdateStadiumDto): Promise<StadiumDto> => {
    const response = await api.put<StadiumDto>(`/stadiums/${id}`, data);
    return response.data;
  },

  /**
   * Delete a stadium
   */
  delete: async (id: number): Promise<void> => {
    await api.delete(`/stadiums/${id}`);
  },

  /**
   * Check if stadium exists
   */
  exists: async (id: number): Promise<boolean> => {
    try {
      await api.head(`/stadiums/${id}`);
      return true;
    } catch {
      return false;
    }
  },
};

// Made with Bob
