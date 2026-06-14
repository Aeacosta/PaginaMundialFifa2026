import { Box, Paper, Typography, Fade, Grow } from '@mui/material';
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
      <Fade in timeout={600}>
        <Paper
          sx={{
            p: 3,
            mb: 3,
            background: 'linear-gradient(135deg, rgba(105, 108, 255, 0.05) 0%, rgba(3, 195, 236, 0.02) 100%)',
            border: '1px solid',
            borderColor: 'divider',
            transition: 'all 0.3s ease-in-out',
            '&:hover': {
              transform: 'translateY(-2px)',
              boxShadow: '0 8px 24px rgba(15, 20, 34, 0.5)',
            },
          }}
        >
          <Typography
            variant="h6"
            gutterBottom
            sx={{
              mb: 2,
              fontWeight: 600,
              background: 'linear-gradient(135deg, #696cff 0%, #03c3ec 100%)',
              WebkitBackgroundClip: 'text',
              WebkitTextFillColor: 'transparent',
              backgroundClip: 'text',
            }}
          >
            ⚡ Upcoming Matches
          </Typography>
          {matchesLoading ? (
            <Loading message="Loading matches..." />
          ) : upcomingMatches && upcomingMatches.length > 0 ? (
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              {upcomingMatches.map((match, index) => (
                <Grow
                  key={match.id}
                  in
                  timeout={300 + index * 100}
                  style={{ transformOrigin: '0 0 0' }}
                >
                  <Paper
                    variant="outlined"
                    sx={{
                      p: 2,
                      display: 'flex',
                      justifyContent: 'space-between',
                      alignItems: 'center',
                      borderRadius: 2,
                      transition: 'all 0.2s ease-in-out',
                      cursor: 'pointer',
                      '&:hover': {
                        bgcolor: 'rgba(105, 108, 255, 0.08)',
                        transform: 'translateX(8px)',
                        borderColor: 'primary.main',
                      },
                    }}
                  >
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                      <Typography variant="body2" color="text.secondary">
                        {new Date(match.matchDate).toLocaleDateString()}
                      </Typography>
                      <Typography variant="body1" sx={{ fontWeight: 600 }}>
                        {match.homeTeamName || 'TBD'} vs {match.awayTeamName || 'TBD'}
                      </Typography>
                    </Box>
                    <Box sx={{ textAlign: 'right' }}>
                      <Typography variant="body2" color="text.secondary">
                        {match.stadiumName}
                      </Typography>
                      <Typography variant="caption" color="primary.main" sx={{ fontWeight: 500 }}>
                        {match.phaseName || match.phase}
                      </Typography>
                    </Box>
                  </Paper>
                </Grow>
              ))}
            </Box>
          ) : (
            <Typography color="text.secondary">
              No upcoming matches scheduled
            </Typography>
          )}
        </Paper>
      </Fade>

      {/* Tournament Information */}
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: { xs: '1fr', md: 'repeat(2, 1fr)' },
          gap: 3,
        }}
      >
        <Fade in timeout={800}>
          <Paper
            sx={{
              p: 3,
              background: 'linear-gradient(135deg, rgba(113, 221, 55, 0.05) 0%, rgba(105, 108, 255, 0.02) 100%)',
              border: '1px solid',
              borderColor: 'divider',
              transition: 'all 0.3s ease-in-out',
              '&:hover': {
                transform: 'translateY(-4px)',
                boxShadow: '0 8px 24px rgba(15, 20, 34, 0.5)',
                borderColor: 'success.main',
              },
            }}
          >
            <Typography
              variant="h6"
              gutterBottom
              sx={{
                fontWeight: 600,
                display: 'flex',
                alignItems: 'center',
                gap: 1,
              }}
            >
              🏆 Tournament Format
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 2, lineHeight: 1.7 }}>
              The 2026 FIFA World Cup will be the 23rd FIFA World Cup, featuring 48 teams
              for the first time in history.
            </Typography>
            <Typography variant="body2" color="text.secondary" component="div" sx={{ lineHeight: 2 }}>
              • Group Stage: 12 groups of 4 teams<br />
              • Round of 32: Top 2 from each group + 8 best third-placed teams<br />
              • Knockout Stage: Round of 32, Round of 16, Quarter-finals, Semi-finals, Final
            </Typography>
          </Paper>
        </Fade>
        <Fade in timeout={1000}>
          <Paper
            sx={{
              p: 3,
              background: 'linear-gradient(135deg, rgba(3, 195, 236, 0.05) 0%, rgba(255, 171, 0, 0.02) 100%)',
              border: '1px solid',
              borderColor: 'divider',
              transition: 'all 0.3s ease-in-out',
              '&:hover': {
                transform: 'translateY(-4px)',
                boxShadow: '0 8px 24px rgba(15, 20, 34, 0.5)',
                borderColor: 'info.main',
              },
            }}
          >
            <Typography
              variant="h6"
              gutterBottom
              sx={{
                fontWeight: 600,
                display: 'flex',
                alignItems: 'center',
                gap: 1,
              }}
            >
              🌎 Host Countries
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 2, lineHeight: 1.7 }}>
              The tournament will be jointly hosted by three countries across North America.
            </Typography>
            <Typography variant="body2" color="text.secondary" component="div" sx={{ lineHeight: 2 }}>
              🇺🇸 United States - 11 stadiums<br />
              🇲🇽 Mexico - 3 stadiums<br />
              🇨🇦 Canada - 2 stadiums
            </Typography>
          </Paper>
        </Fade>
      </Box>
    </Box>
  );
};

export default DashboardPage;

// Made with Bob
