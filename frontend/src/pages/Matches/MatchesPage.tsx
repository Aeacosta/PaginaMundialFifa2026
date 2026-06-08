import { useState } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Chip,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Avatar,
} from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { matchesService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';
import { MatchPhase, MatchStatus } from '../../types';

const MatchesPage = () => {
  const navigate = useNavigate();
  const [selectedPhase, setSelectedPhase] = useState<string>('');
  const [selectedStatus, setSelectedStatus] = useState<string>('');

  // Fetch matches
  const { data: matches, isLoading, error, refetch } = useQuery({
    queryKey: ['matches', selectedPhase, selectedStatus],
    queryFn: async () => {
      if (selectedPhase) {
        return matchesService.getByPhase(selectedPhase as MatchPhase);
      }
      if (selectedStatus) {
        return matchesService.getByStatus(selectedStatus as MatchStatus);
      }
      const result = await matchesService.getAll({ page: 1, pageSize: 100 });
      return result.items;
    },
  });

  const handleMatchClick = (matchId: number) => {
    navigate(`/matches/${matchId}`);
  };

  const handleClearFilters = () => {
    setSelectedPhase('');
    setSelectedStatus('');
  };

  if (error) {
    return (
      <ErrorMessage
        title="Failed to load matches"
        message={error instanceof Error ? error.message : 'An error occurred'}
        onRetry={refetch}
      />
    );
  }

  const phases = Object.values(MatchPhase);
  const statuses = Object.values(MatchStatus);

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
        title="Matches"
        subtitle={`${matches?.length || 104} matches in FIFA World Cup 2026`}
      />

      {/* Filters */}
      <Box
        sx={{
          display: 'flex',
          gap: 2,
          mb: 3,
          flexWrap: 'wrap',
        }}
      >
        <FormControl sx={{ minWidth: 200 }}>
          <InputLabel>Phase</InputLabel>
          <Select
            value={selectedPhase}
            label="Phase"
            onChange={(e) => setSelectedPhase(e.target.value)}
          >
            <MenuItem value="">All Phases</MenuItem>
            {phases.map((phase) => (
              <MenuItem key={phase} value={phase}>
                {phase.replace(/([A-Z])/g, ' $1').trim()}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <FormControl sx={{ minWidth: 200 }}>
          <InputLabel>Status</InputLabel>
          <Select
            value={selectedStatus}
            label="Status"
            onChange={(e) => setSelectedStatus(e.target.value)}
          >
            <MenuItem value="">All Statuses</MenuItem>
            {statuses.map((status) => (
              <MenuItem key={status} value={status}>
                {status}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        {(selectedPhase || selectedStatus) && (
          <Chip
            label="Clear Filters"
            onDelete={handleClearFilters}
            color="primary"
            variant="outlined"
          />
        )}
      </Box>

      {/* Matches List */}
      {isLoading ? (
        <Loading message="Loading matches..." />
      ) : matches && matches.length > 0 ? (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          {matches.map((match) => (
            <Card
              key={match.id}
              sx={{
                cursor: 'pointer',
                transition: 'transform 0.2s, box-shadow 0.2s',
                '&:hover': {
                  transform: 'translateX(4px)',
                  boxShadow: 2,
                },
              }}
              onClick={() => handleMatchClick(match.id)}
            >
              <CardContent>
                <Box
                  sx={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    flexWrap: 'wrap',
                    gap: 2,
                  }}
                >
                  {/* Match Info */}
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 3, flexGrow: 1 }}>
                    <Box sx={{ minWidth: 120 }}>
                      <Typography variant="body2" color="text.secondary">
                        {new Date(match.matchDate).toLocaleDateString()}
                      </Typography>
                      <Typography variant="caption" color="text.secondary">
                        {new Date(match.matchDate).toLocaleTimeString([], {
                          hour: '2-digit',
                          minute: '2-digit',
                        })}
                      </Typography>
                    </Box>

                    <Box sx={{ flexGrow: 1 }}>
                      <Box
                        sx={{
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'space-between',
                          mb: 0.5,
                        }}
                      >
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                          <Avatar
                            src={match.homeTeamFlagUrl}
                            alt={match.homeTeamName}
                            sx={{
                              width: 28,
                              height: 28,
                              border: '1px solid',
                              borderColor: 'divider'
                            }}
                            variant="rounded"
                          >
                            {match.homeTeamCode?.substring(0, 2) || '??'}
                          </Avatar>
                          <Typography variant="body1" sx={{ fontWeight: 'medium' }}>
                            {match.homeTeamName || 'TBD'}
                          </Typography>
                        </Box>
                        {match.result && (
                          <Typography variant="h6" sx={{ mx: 2 }}>
                            {match.result.homeTeamScore}
                          </Typography>
                        )}
                      </Box>
                      <Box
                        sx={{
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'space-between',
                        }}
                      >
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                          <Avatar
                            src={match.awayTeamFlagUrl}
                            alt={match.awayTeamName}
                            sx={{
                              width: 28,
                              height: 28,
                              border: '1px solid',
                              borderColor: 'divider'
                            }}
                            variant="rounded"
                          >
                            {match.awayTeamCode?.substring(0, 2) || '??'}
                          </Avatar>
                          <Typography variant="body1" sx={{ fontWeight: 'medium' }}>
                            {match.awayTeamName || 'TBD'}
                          </Typography>
                        </Box>
                        {match.result && (
                          <Typography variant="h6" sx={{ mx: 2 }}>
                            {match.result.awayTeamScore}
                          </Typography>
                        )}
                      </Box>
                    </Box>
                  </Box>

                  {/* Match Details */}
                  <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1, alignItems: 'flex-end' }}>
                    <Chip
                      label={match.status}
                      size="small"
                      color={getStatusColor(match.status)}
                    />
                    <Typography variant="caption" color="text.secondary">
                      {match.phaseName}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      {match.stadiumName}
                    </Typography>
                    {match.groupName && (
                      <Chip label={`Group ${match.groupName}`} size="small" variant="outlined" />
                    )}
                  </Box>
                </Box>
              </CardContent>
            </Card>
          ))}
        </Box>
      ) : (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h6" color="text.secondary">
            No matches found
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
            Try adjusting your filters
          </Typography>
        </Box>
      )}
    </Box>
  );
};

export default MatchesPage;

// Made with Bob
