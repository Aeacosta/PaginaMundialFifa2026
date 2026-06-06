import { Box, Card, CardContent, Chip, Paper, Typography, Divider } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { useParams, useNavigate } from 'react-router-dom';
import { teamsService, standingsService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { IconButton } from '@mui/material';

const TeamDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const teamId = parseInt(id || '0');

  // Fetch team details
  const { data: team, isLoading: teamLoading, error: teamError } = useQuery({
    queryKey: ['team', teamId],
    queryFn: () => teamsService.getById(teamId),
    enabled: !!teamId,
  });

  // Fetch team standings
  const { data: standing, isLoading: standingsLoading } = useQuery({
    queryKey: ['team-standings', teamId],
    queryFn: () => standingsService.getByTeam(teamId),
    enabled: !!teamId,
  });

  const isLoading = teamLoading || standingsLoading;

  if (teamError) {
    return (
      <ErrorMessage
        title="Failed to load team details"
        message={teamError instanceof Error ? teamError.message : 'An error occurred'}
      />
    );
  }

  if (isLoading) {
    return <Loading message="Loading team details..." />;
  }

  if (!team) {
    return (
      <ErrorMessage
        title="Team not found"
        message="The requested team could not be found."
      />
    );
  }

  return (
    <Box>
      <PageHeader
        title={team.name}
        subtitle={`${team.code} • ${team.confederation}`}
        breadcrumbs={[
          { label: 'Home', path: '/' },
          { label: 'Teams', path: '/teams' },
          { label: team.name },
        ]}
        action={
          <IconButton onClick={() => navigate('/teams')} color="primary">
            <ArrowBackIcon />
          </IconButton>
        }
      />

      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: { xs: '1fr', md: 'repeat(2, 1fr)' },
          gap: 3,
        }}
      >
        {/* Team Information */}
        <Card>
          <CardContent>
            <Typography variant="h6" gutterBottom>
              Team Information
            </Typography>
            <Divider sx={{ mb: 2 }} />
            
            {team.flagUrl && (
              <Box
                component="img"
                src={team.flagUrl}
                alt={`${team.name} flag`}
                sx={{
                  width: '100%',
                  maxWidth: 300,
                  height: 'auto',
                  mb: 3,
                  borderRadius: 1,
                }}
              />
            )}

            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <Box>
                <Typography variant="body2" color="text.secondary">
                  Full Name
                </Typography>
                <Typography variant="body1" sx={{ fontWeight: 'medium' }}>
                  {team.name}
                </Typography>
              </Box>

              <Box>
                <Typography variant="body2" color="text.secondary">
                  Team Code
                </Typography>
                <Typography variant="body1" sx={{ fontWeight: 'medium' }}>
                  {team.code}
                </Typography>
              </Box>

              <Box>
                <Typography variant="body2" color="text.secondary">
                  Confederation
                </Typography>
                <Chip
                  label={team.confederation}
                  color="primary"
                  size="small"
                  sx={{ mt: 0.5 }}
                />
              </Box>

              {team.groupName && (
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Group
                  </Typography>
                  <Chip
                    label={`Group ${team.groupName}`}
                    color="secondary"
                    size="small"
                    sx={{ mt: 0.5 }}
                  />
                </Box>
              )}

              {team.fifaRanking && (
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    FIFA Ranking
                  </Typography>
                  <Typography variant="h4" color="primary" sx={{ mt: 0.5 }}>
                    #{team.fifaRanking}
                  </Typography>
                </Box>
              )}
            </Box>
          </CardContent>
        </Card>

        {/* Team Statistics */}
        <Card>
          <CardContent>
            <Typography variant="h6" gutterBottom>
              Tournament Statistics
            </Typography>
            <Divider sx={{ mb: 2 }} />

            {standing ? (
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <Box
                  sx={{
                    display: 'grid',
                    gridTemplateColumns: 'repeat(2, 1fr)',
                    gap: 2,
                  }}
                >
                  <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                    <Typography variant="h3" color="primary">
                      {standing.position}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Position
                    </Typography>
                  </Paper>

                  <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                    <Typography variant="h3" color="primary">
                      {standing.points}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Points
                    </Typography>
                  </Paper>

                  <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                    <Typography variant="h3">{standing.played}</Typography>
                    <Typography variant="body2" color="text.secondary">
                      Played
                    </Typography>
                  </Paper>

                  <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                    <Typography variant="h3" color="success.main">
                      {standing.won}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Won
                    </Typography>
                  </Paper>

                  <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                    <Typography variant="h3" color="warning.main">
                      {standing.drawn}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Drawn
                    </Typography>
                  </Paper>

                  <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                    <Typography variant="h3" color="error.main">
                      {standing.lost}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Lost
                    </Typography>
                  </Paper>

                  <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                    <Typography variant="h3">{standing.goalsFor}</Typography>
                    <Typography variant="body2" color="text.secondary">
                      Goals For
                    </Typography>
                  </Paper>

                  <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                    <Typography variant="h3">{standing.goalsAgainst}</Typography>
                    <Typography variant="body2" color="text.secondary">
                      Goals Against
                    </Typography>
                  </Paper>
                </Box>

                <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                  <Typography variant="h3" color="primary">
                    {standing.goalDifference > 0 ? '+' : ''}
                    {standing.goalDifference}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Goal Difference
                  </Typography>
                </Paper>
              </Box>
            ) : (
              <Typography color="text.secondary">
                No statistics available yet. Statistics will be updated once matches are played.
              </Typography>
            )}
          </CardContent>
        </Card>
      </Box>
    </Box>
  );
};

export default TeamDetailsPage;

// Made with Bob
