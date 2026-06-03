// Enums
export enum Confederation {
  UEFA = 'UEFA',
  CONMEBOL = 'CONMEBOL',
  CONCACAF = 'CONCACAF',
  CAF = 'CAF',
  AFC = 'AFC',
  OFC = 'OFC',
}

export enum MatchPhase {
  GroupStage = 'GroupStage',
  RoundOf32 = 'RoundOf32',
  RoundOf16 = 'RoundOf16',
  QuarterFinals = 'QuarterFinals',
  SemiFinals = 'SemiFinals',
  ThirdPlace = 'ThirdPlace',
  Final = 'Final',
}

export enum MatchStatus {
  Scheduled = 'Scheduled',
  InProgress = 'InProgress',
  Completed = 'Completed',
  Postponed = 'Postponed',
  Cancelled = 'Cancelled',
}

// DTOs
export interface TeamDto {
  id: number;
  name: string;
  code: string;
  flagUrl?: string;
  groupId?: number;
  groupName?: string;
  confederation: Confederation;
  fifaRanking?: number;
}

export interface GroupDto {
  id: number;
  name: string;
  description?: string;
}

export interface StadiumDto {
  id: number;
  name: string;
  city: string;
  country: string;
  capacity: number;
  imageUrl?: string;
  latitude?: number;
  longitude?: number;
}

export interface MatchDto {
  id: number;
  homeTeamId: number;
  homeTeamName: string;
  homeTeamCode: string;
  awayTeamId: number;
  awayTeamName: string;
  awayTeamCode: string;
  stadiumId: number;
  stadiumName: string;
  matchDate: string;
  phase: MatchPhase;
  round?: number;
  groupId?: number;
  groupName?: string;
  status: MatchStatus;
  result?: MatchResultDto;
}

export interface MatchResultDto {
  id: number;
  matchId: number;
  homeTeamScore: number;
  awayTeamScore: number;
  homeTeamPenalties?: number;
  awayTeamPenalties?: number;
  winnerTeamId?: number;
  winnerTeamName?: string;
}

export interface StandingDto {
  id: number;
  teamId: number;
  teamName: string;
  teamCode: string;
  groupId: number;
  groupName: string;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  points: number;
  position: number;
}

export interface GroupWithStandingsDto {
  group: GroupDto;
  standings: StandingDto[];
}

export interface DashboardDto {
  totalTeams: number;
  totalMatches: number;
  completedMatches: number;
  upcomingMatches: number;
  totalGoals: number;
  topScorers: TopScorerDto[];
  recentResults: MatchDto[];
  upcomingMatchesList: MatchDto[];
}

export interface TopScorerDto {
  teamId: number;
  teamName: string;
  teamCode: string;
  goals: number;
}

export interface TeamPerformanceDto {
  teamId: number;
  teamName: string;
  teamCode: string;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  points: number;
  winPercentage: number;
}

// Pagination
export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

// API Response wrapper
export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

// Made with Bob
