import { Box, Card, CardContent, Typography, Paper, Chip, IconButton, Divider } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { useParams, useNavigate } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { matchesService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';
import { MatchStatus } from '../../types';

const MatchDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const matchId = parseInt(id || '0');

  // Fetch match details
  const { data: match, isLoading, error } = useQuery({
    queryKey: ['match', matchId],
    queryFn: () => matchesService.getById(matchId),
    enabled: !!matchId,
  });

  if (error) {
    return (
      <ErrorMessage
        title="Failed to load match details"
        message={error instanceof Error ? error.message : 'An error occurred'}
      />
    );
  }

  if (isLoading) {
    return <Loading message="Loading match details..." />;
  }

  if (!match) {
    return (
      <ErrorMessage
        title="Match not found"
        message="The requested match could not be found."
      />
    );
  }

  const getStatusColor = (status: MatchStatus) => {
    switch (status) {
      case MatchStatus.Completed:
        return 'success';
      case MatchStatus.InProgress:
        return 'warning';
      case MatchStatus.Scheduled:
        return 'info';
      case MatchStatus.Postponed:
        return 'default';
      case MatchStatus.Cancelled:
        return 'error';
      default:
        return 'default';
    }
  };

  return (
    <Box>
      <PageHeader
        title="Match Details"
        subtitle={`${match.phaseName}${
          match.groupName ? ` - Group ${match.groupName}` : ''
        }`}
        breadcrumbs={[
          { label: 'Home', path: '/' },
          { label: 'Matches', path: '/matches' },
          { label: 'Match Details' },
        ]}
        action={
          <IconButton onClick={() => navigate('/matches')} color="primary">
            <ArrowBackIcon />
          </IconButton>
        }
      />

      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: { xs: '1fr', md: '2fr 1fr' },
          gap: 3,
        }}
      >
        {/* Match Score */}
        <Card>
          <CardContent>
            <Box sx={{ textAlign: 'center', mb: 3 }}>
              <Chip
                label={match.status}
                color={getStatusColor(match.status)}
                sx={{ mb: 2 }}
              />
              <Typography variant="body2" color="text.secondary">
                {new Date(match.matchDate).toLocaleDateString('en-US', {
                  weekday: 'long',
                  year: 'numeric',
                  month: 'long',
                  day: 'numeric',
                })}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {new Date(match.matchDate).toLocaleTimeString([], {
                  hour: '2-digit',
                  minute: '2-digit',
                })}
              </Typography>
            </Box>

            <Box
              sx={{
                display: 'flex',
                justifyContent: 'space-around',
                alignItems: 'center',
                py: 4,
              }}
            >
              {/* Home Team */}
              <Box sx={{ textAlign: 'center', flex: 1 }}>
                <Typography variant="h5" gutterBottom>
                  {match.homeTeamName || 'TBD'}
                </Typography>
                <Typography variant="caption" color="text.secondary">
                  {match.homeTeamCode || ''}
                </Typography>
              </Box>

              {/* Score */}
              <Box sx={{ textAlign: 'center', minWidth: 120 }}>
                {match.result ? (
                  <>
                    <Box sx={{ display: 'flex', gap: 2, alignItems: 'center' }}>
                      <Typography variant="h2" color="primary">
                        {match.result.homeTeamScore}
                      </Typography>
                      <Typography variant="h4" color="text.secondary">
                        -
                      </Typography>
                      <Typography variant="h2" color="primary">
                        {match.result.awayTeamScore}
                      </Typography>
                    </Box>
                    {(match.result.homeTeamPenalties !== null ||
                      match.result.awayTeamPenalties !== null) && (
                      <Typography variant="caption" color="text.secondary">
                        Penalties: {match.result.homeTeamPenalties} -{' '}
                        {match.result.awayTeamPenalties}
                      </Typography>
                    )}
                  </>
                ) : (
                  <Typography variant="h4" color="text.secondary">
                    VS
                  </Typography>
                )}
              </Box>

              {/* Away Team */}
              <Box sx={{ textAlign: 'center', flex: 1 }}>
                <Typography variant="h5" gutterBottom>
                  {match.awayTeamName || 'TBD'}
                </Typography>
                <Typography variant="caption" color="text.secondary">
                  {match.awayTeamCode || ''}
                </Typography>
              </Box>
            </Box>

            {match.result?.winnerTeamName && (
              <Box sx={{ textAlign: 'center', pt: 2, borderTop: 1, borderColor: 'divider' }}>
                <Typography variant="body2" color="text.secondary">
                  Winner
                </Typography>
                <Typography variant="h6" color="success.main">
                  {match.result.winnerTeamName}
                </Typography>
              </Box>
            )}
          </CardContent>
        </Card>

        {/* Match Information */}
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Match Information
              </Typography>
              <Divider sx={{ mb: 2 }} />
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Phase
                  </Typography>
                  <Typography variant="body1">
                    {match.phaseName}
                  </Typography>
                </Box>

                {match.groupName && (
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      Group
                    </Typography>
                    <Chip label={`Group ${match.groupName}`} size="small" color="secondary" />
                  </Box>
                )}

                {match.round && (
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      Round
                    </Typography>
                    <Typography variant="body1">{match.round}</Typography>
                  </Box>
                )}

                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Stadium
                  </Typography>
                  <Typography variant="body1">{match.stadiumName}</Typography>
                </Box>

                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Date & Time
                  </Typography>
                  <Typography variant="body1">
                    {new Date(match.matchDate).toLocaleString()}
                  </Typography>
                </Box>

                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Status
                  </Typography>
                  <Chip
                    label={match.status}
                    size="small"
                    color={getStatusColor(match.status)}
                  />
                </Box>
              </Box>
            </CardContent>
          </Card>

          <Paper sx={{ p: 2, bgcolor: 'info.light' }}>
            <Typography variant="body2" sx={{ fontWeight: 'bold', mb: 1 }}>
              FIFA World Cup 2026
            </Typography>
            <Typography variant="caption" color="text.secondary">
              This match is part of the FIFA World Cup 2026, jointly hosted by the United States,
              Mexico, and Canada.
            </Typography>
          </Paper>
        </Box>
      </Box>
    </Box>
  );
};

export default MatchDetailsPage;

// Made with Bob
