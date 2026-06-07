import { describe, it, expect, vi, beforeEach } from 'vitest';
import { stadiumsService } from '../stadiums.service';
import { apiClient } from '../../../config/api';
import { mockStadium, mockStadiums, mockPagedStadiums } from '../../../test/mockData';

// Mock the API client
vi.mock('../../../config/api', () => ({
  apiClient: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    delete: vi.fn(),
  },
}));

describe('stadiumsService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('getAll', () => {
    it('fetches all stadiums with default pagination', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockPagedStadiums });

      const result = await stadiumsService.getAll();

      expect(apiClient.get).toHaveBeenCalledWith('/stadiums', {
        params: undefined,
      });
      expect(result).toEqual(mockPagedStadiums);
    });

    it('fetches all stadiums with custom pagination', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockPagedStadiums });

      await stadiumsService.getAll({ page: 2, pageSize: 20 });

      expect(apiClient.get).toHaveBeenCalledWith('/stadiums', {
        params: { page: 2, pageSize: 20 },
      });
    });
  });

  describe('getById', () => {
    it('fetches a stadium by ID', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockStadium });

      const result = await stadiumsService.getById(1);

      expect(apiClient.get).toHaveBeenCalledWith('/stadiums/1');
      expect(result).toEqual(mockStadium);
    });
  });

  describe('getByCity', () => {
    it('fetches stadiums by city', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockStadiums });

      const result = await stadiumsService.getByCity('East Rutherford');

      expect(apiClient.get).toHaveBeenCalledWith('/stadiums/city/East Rutherford');
      expect(result).toEqual(mockStadiums);
    });
  });

  describe('getByCountry', () => {
    it('fetches stadiums by country', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockStadiums });

      const result = await stadiumsService.getByCountry('USA');

      expect(apiClient.get).toHaveBeenCalledWith('/stadiums/country/USA');
      expect(result).toEqual(mockStadiums);
    });
  });

  describe('search', () => {
    it('searches stadiums by search term', async () => {
      vi.mocked(apiClient.get).mockResolvedValue({ data: mockStadiums });

      const result = await stadiumsService.search('MetLife');

      expect(apiClient.get).toHaveBeenCalledWith('/stadiums/search', {
        params: { searchTerm: 'MetLife' },
      });
      expect(result).toEqual(mockStadiums);
    });
  });

  describe('create', () => {
    it('creates a new stadium', async () => {
      const newStadium = {
        name: 'MetLife Stadium',
        city: 'East Rutherford',
        country: 'USA',
        capacity: 82500,
      };
      
      vi.mocked(apiClient.post).mockResolvedValue({ data: mockStadium });

      const result = await stadiumsService.create(newStadium);

      expect(apiClient.post).toHaveBeenCalledWith('/stadiums', newStadium);
      expect(result).toEqual(mockStadium);
    });
  });

  describe('update', () => {
    it('updates an existing stadium', async () => {
      const updates = {
        name: 'MetLife Stadium',
        city: 'East Rutherford',
        country: 'USA',
        capacity: 90000,
      };
      vi.mocked(apiClient.put).mockResolvedValue({ data: { ...mockStadium, ...updates } });

      const result = await stadiumsService.update(1, updates);

      expect(apiClient.put).toHaveBeenCalledWith('/stadiums/1', updates);
      expect(result.capacity).toBe(90000);
    });
  });

  describe('delete', () => {
    it('deletes a stadium', async () => {
      vi.mocked(apiClient.delete).mockResolvedValue({ data: undefined });

      await stadiumsService.delete(1);

      expect(apiClient.delete).toHaveBeenCalledWith('/stadiums/1');
    });
  });
});

// Made with Bob