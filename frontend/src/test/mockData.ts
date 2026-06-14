import type {
  TeamDto,
  GroupDto,
  StadiumDto,
  MatchDto,
  StandingDto,
  DashboardDto,
  PagedResult,
} from '../types';

import {
  Confederation,
  MatchPhase,
  MatchStatus,
} from '../types';

// Mock Teams
export const mockTeam: TeamDto = {
  id: 1,
  name: 'Brazil',
  code: 'BRA',
  flagUrl: 'https://example.com/flags/bra.png',
  groupId: 1,
  groupName: 'Group A',
  confederation: Confederation.CONMEBOL,
  confederationName: 'CONMEBOL',
  fifaRanking: 1,
};

export const mockTeams: TeamDto[] = [
  mockTeam,
  {
    id: 2,
    name: 'Argentina',
    code: 'ARG',
    flagUrl: 'https://example.com/flags/arg.png',
    groupId: 1,
    groupName: 'Group A',
    confederation: Confederation.CONMEBOL,
    confederationName: 'CONMEBOL',
    fifaRanking: 2,
  },
  {
    id: 3,
    name: 'Germany',
    code: 'GER',
    flagUrl: 'https://example.com/flags/ger.png',
    groupId: 2,
    groupName: 'Group B',
    confederation: Confederation.UEFA,
    confederationName: 'UEFA',
    fifaRanking: 3,
  },
];

export const mockPagedTeams: PagedResult<TeamDto> = {
  items: mockTeams,
  pageNumber: 1,
  pageSize: 10,
  totalPages: 1,
  totalCount: 3,
  hasPreviousPage: false,
  hasNextPage: false,
};

// Mock Groups
export const mockGroup: GroupDto = {
  id: 1,
  name: 'Group A',
  description: 'Group A teams',
};

export const mockGroups: GroupDto[] = [
  mockGroup,
  {
    id: 2,
    name: 'Group B',
    description: 'Group B teams',
  },
];

// Mock Stadiums
export const mockStadium: StadiumDto = {
  id: 1,
  name: 'MetLife Stadium',
  city: 'East Rutherford',
  country: 'USA',
  capacity: 82500,
  imageUrl: 'https://example.com/stadiums/metlife.jpg',
  latitude: 40.8128,
  longitude: -74.0742,
};

export const mockStadiums: StadiumDto[] = [
  mockStadium,
  {
    id: 2,
    name: 'AT&T Stadium',
    city: 'Arlington',
    country: 'USA',
    capacity: 80000,
    imageUrl: 'https://example.com/stadiums/att.jpg',
    latitude: 32.7473,
    longitude: -97.0945,
  },
];

export const mockPagedStadiums: PagedResult<StadiumDto> = {
  items: mockStadiums,
  pageNumber: 1,
  pageSize: 10,
  totalPages: 1,
  totalCount: 2,
  hasPreviousPage: false,
  hasNextPage: false,
};

// Mock Matches
export const mockMatch: MatchDto = {
  id: 1,
  homeTeamId: 1,
  homeTeamName: 'Brazil',
  homeTeamCode: 'BRA',
  homeTeamFlagUrl: 'https://example.com/flags/bra.png',
  awayTeamId: 2,
  awayTeamName: 'Argentina',
  awayTeamCode: 'ARG',
  awayTeamFlagUrl: 'https://example.com/flags/arg.png',
  stadiumId: 1,
  stadiumName: 'MetLife Stadium',
  stadiumCity: 'East Rutherford',
  stadiumCountry: 'USA',
  matchDate: '2026-06-15T20:00:00Z',
  phase: MatchPhase.GroupStage,
  phaseName: 'Group Stage',
  round: 1,
  groupId: 1,
  groupName: 'Group A',
  status: MatchStatus.Scheduled,
  statusName: 'Scheduled',
  result: {
    id: 1,
    matchId: 1,
    homeTeamScore: 2,
    awayTeamScore: 1,
    winnerTeamId: 1,
    winnerTeamName: 'Brazil',
  },
};

export const mockMatches: MatchDto[] = [
  mockMatch,
  {
    ...mockMatch,
    id: 2,
    homeTeamId: 3,
    homeTeamName: 'Germany',
    homeTeamCode: 'GER',
    awayTeamId: 1,
    awayTeamName: 'Brazil',
    awayTeamCode: 'BRA',
    status: MatchStatus.Completed,
    statusName: 'Completed',
  },
];

export const mockPagedMatches: PagedResult<MatchDto> = {
  items: mockMatches,
  pageNumber: 1,
  pageSize: 10,
  totalPages: 1,
  totalCount: 2,
  hasPreviousPage: false,
  hasNextPage: false,
};

// Mock Standings
export const mockStanding: StandingDto = {
  id: 1,
  teamId: 1,
  teamName: 'Brazil',
  teamCode: 'BRA',
  groupId: 1,
  groupName: 'Group A',
  played: 3,
  won: 2,
  drawn: 1,
  lost: 0,
  goalsFor: 6,
  goalsAgainst: 2,
  goalDifference: 4,
  points: 7,
  position: 1,
};

export const mockStandings: StandingDto[] = [
  mockStanding,
  {
    id: 2,
    teamId: 2,
    teamName: 'Argentina',
    teamCode: 'ARG',
    groupId: 1,
    groupName: 'Group A',
    played: 3,
    won: 2,
    drawn: 0,
    lost: 1,
    goalsFor: 5,
    goalsAgainst: 3,
    goalDifference: 2,
    points: 6,
    position: 2,
  },
];

// Mock Dashboard
export const mockDashboard: DashboardDto = {
  totalTeams: 48,
  totalMatches: 104,
  completedMatches: 10,
  upcomingMatches: 94,
  totalGoals: 25,
  topScorers: [
    {
      teamId: 1,
      teamName: 'Brazil',
      teamCode: 'BRA',
      goals: 8,
    },
    {
      teamId: 2,
      teamName: 'Argentina',
      teamCode: 'ARG',
      goals: 7,
    },
  ],
  recentResults: [mockMatch],
  upcomingMatchesList: [mockMatches[1]],
};

// Made with Bob