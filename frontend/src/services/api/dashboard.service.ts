import api from '../../config/api';
import type { DashboardDto } from '../../types';

export const dashboardService = {
  /**
   * Get dashboard statistics and data
   */
  getStats: async (): Promise<DashboardDto> => {
    const response = await api.get<DashboardDto>('/dashboard');
    return response.data;
  },
};

// Made with Bob
