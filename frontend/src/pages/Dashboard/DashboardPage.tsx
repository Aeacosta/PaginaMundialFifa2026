import { Box, Paper, Typography } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import SportsIcon from '@mui/icons-material/Sports';
import StadiumIcon from '@mui/icons-material/Stadium';
import GroupsIcon from '@mui/icons-material/Groups';
import EmojiEventsIcon from '@mui/icons-material/EmojiEvents';
import { dashboardService, matchesService } from '../../services/api';
import { PageHeader, StatCard, Loading, ErrorMessage } from '../../components/shared';

const DashboardPage = () => {
  // Fetch dashboard statistics
  const { data: stats, isLoading: statsLoading, error: statsError } = useQuery({
    queryKey: ['dashboard-stats'],
    queryFn: dashboardService.getStats,
  });

  // Fetch upcoming matches
  const { data: upcomingMatches, isLoading: matchesLoading } = useQuery({
    queryKey: ['upcoming-matches'],
    queryFn: () => matchesService.getUpcoming(5),
  });

  const isLoading = statsLoading || matchesLoading;

  if (statsError) {
    return (
      <ErrorMessage
        title="Failed to load dashboard"
        message={statsError instanceof Error ? statsError.message : 'An error occurred'}
      />
    );
  }

  return (
    <Box>
      <PageHeader
        title="Dashboard"
        subtitle="FIFA World Cup 2026 - Overview and Statistics"
      />

      {/* Statistics Cards */}
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: {
            xs: '1fr',
            sm: 'repeat(2, 1fr)',
            md: 'repeat(4, 1fr)',
          },
          gap: 3,
          mb: 4,
        }}
      >
        <StatCard
          title="Total Teams"
          value={stats?.totalTeams || 48}
          icon={<GroupsIcon fontSize="large" />}
          loading={isLoading}
          color="primary"
        />
        <StatCard
          title="Total Matches"
          value={stats?.totalMatches || 104}
          icon={<SportsIcon fontSize="large" />}
          loading={isLoading}
          color="secondary"
        />
        <StatCard
          title="Stadiums"
          value={16}
          icon={<StadiumIcon fontSize="large" />}
          loading={isLoading}
          color="success"
        />
        <StatCard
          title="Groups"
          value={12}
          icon={<EmojiEventsIcon fontSize="large" />}
          loading={isLoading}
          color="warning"
        />
      </Box>

      {/* Upcoming Matches */}
      <Paper sx={{ p: 3, mb: 3 }}>
        <Typography variant="h6" gutterBottom sx={{ mb: 2 }}>
          Upcoming Matches
        </Typography>
        {matchesLoading ? (
          <Loading message="Loading matches..." />
        ) : upcomingMatches && upcomingMatches.length > 0 ? (
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            {upcomingMatches.map((match) => (
              <Paper
                key={match.id}
                variant="outlined"
                sx={{
                  p: 2,
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  '&:hover': {
                    bgcolor: 'action.hover',
                  },
                }}
              >
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                  <Typography variant="body2" color="text.secondary">
                    {new Date(match.matchDate).toLocaleDateString()}
                  </Typography>
                  <Typography variant="body1" sx={{ fontWeight: 'medium' }}>
                    {match.homeTeamName || 'TBD'} vs {match.awayTeamName || 'TBD'}
                  </Typography>
                </Box>
                <Box sx={{ textAlign: 'right' }}>
                  <Typography variant="body2" color="text.secondary">
                    {match.stadiumName}
                  </Typography>
                  <Typography variant="caption" color="text.secondary">
                    {match.phase}
                  </Typography>
                </Box>
              </Paper>
            ))}
          </Box>
        ) : (
          <Typography color="text.secondary">
            No upcoming matches scheduled
          </Typography>
        )}
      </Paper>

      {/* Tournament Information */}
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: { xs: '1fr', md: 'repeat(2, 1fr)' },
          gap: 3,
        }}
      >
        <Paper sx={{ p: 3 }}>
          <Typography variant="h6" gutterBottom>
            Tournament Format
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            The 2026 FIFA World Cup will be the 23rd FIFA World Cup, featuring 48 teams
            for the first time in history.
          </Typography>
          <Typography variant="body2" color="text.secondary" component="div">
            • Group Stage: 12 groups of 4 teams<br />
            • Round of 32: Top 2 from each group + 8 best third-placed teams<br />
            • Knockout Stage: Round of 32, Round of 16, Quarter-finals, Semi-finals, Final
          </Typography>
        </Paper>
        <Paper sx={{ p: 3 }}>
          <Typography variant="h6" gutterBottom>
            Host Countries
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            The tournament will be jointly hosted by three countries across North America.
          </Typography>
          <Typography variant="body2" color="text.secondary" component="div">
            🇺🇸 United States - 11 stadiums<br />
            🇲🇽 Mexico - 3 stadiums<br />
            🇨🇦 Canada - 2 stadiums
          </Typography>
        </Paper>
      </Box>
    </Box>
  );
};

export default DashboardPage;

// Made with Bob
