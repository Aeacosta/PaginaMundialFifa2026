import { useState } from 'react';
import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Chip,
  Typography,
  Avatar,
} from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { standingsService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';

const StandingsPage = () => {
  const navigate = useNavigate();
  const [selectedGroup, setSelectedGroup] = useState<string>('');

  // Fetch standings
  const { data: standings, isLoading, error, refetch } = useQuery({
    queryKey: ['standings', selectedGroup],
    queryFn: async () => {
      if (selectedGroup) {
        const groupId = selectedGroup.charCodeAt(0) - 64; // Convert 'A' to 1, 'B' to 2, etc.
        return standingsService.getByGroup(groupId);
      }
      return standingsService.getAll();
    },
  });

  const handleTeamClick = (teamId: number) => {
    navigate(`/teams/${teamId}`);
  };

  if (error) {
    return (
      <ErrorMessage
        title="Failed to load standings"
        message={error instanceof Error ? error.message : 'An error occurred'}
        onRetry={refetch}
      />
    );
  }

  const groups = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L'];

  // Group standings by group name
  const groupedStandings = standings?.reduce((acc, standing) => {
    const groupName = standing.groupName;
    if (!acc[groupName]) {
      acc[groupName] = [];
    }
    acc[groupName].push(standing);
    return acc;
  }, {} as Record<string, typeof standings>);

  return (
    <Box sx={{ maxWidth: '1400px', margin: '0 auto', width: '100%' }}>
      <PageHeader
        title="Standings"
        subtitle="FIFA World Cup 2026 - Group Stage Standings"
      />

      {/* Filter */}
      <Box sx={{ mb: 3, display: 'flex', gap: 2, alignItems: 'center' }}>
        <FormControl sx={{ minWidth: 200 }}>
          <InputLabel>Filter by Group</InputLabel>
          <Select
            value={selectedGroup}
            label="Filter by Group"
            onChange={(e) => setSelectedGroup(e.target.value)}
          >
            <MenuItem value="">All Groups</MenuItem>
            {groups.map((group) => (
              <MenuItem key={group} value={group}>
                Group {group}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        {selectedGroup && (
          <Chip
            label="Clear Filter"
            onDelete={() => setSelectedGroup('')}
            color="primary"
            variant="outlined"
          />
        )}
      </Box>

      {/* Standings Tables */}
      {isLoading ? (
        <Loading message="Loading standings..." />
      ) : standings && standings.length > 0 ? (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
          {Object.entries(groupedStandings || {})
            .sort(([a], [b]) => a.localeCompare(b))
            .map(([groupName, groupStandings]) => (
              <Paper key={groupName} sx={{ overflow: 'hidden' }}>
                <Box sx={{ p: 2, bgcolor: 'primary.main', color: 'white' }}>
                  <Typography variant="h6">Group {groupName}</Typography>
                </Box>
                <TableContainer>
                  <Table>
                    <TableHead>
                      <TableRow>
                        <TableCell sx={{ fontWeight: 'bold' }}>Pos</TableCell>
                        <TableCell sx={{ fontWeight: 'bold' }}>Team</TableCell>
                        <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                          P
                        </TableCell>
                        <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                          W
                        </TableCell>
                        <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                          D
                        </TableCell>
                        <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                          L
                        </TableCell>
                        <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                          GF
                        </TableCell>
                        <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                          GA
                        </TableCell>
                        <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                          GD
                        </TableCell>
                        <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                          Pts
                        </TableCell>
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {groupStandings
                        ?.sort((a, b) => a.position - b.position)
                        .map((standing) => (
                          <TableRow
                            key={standing.id}
                            hover
                            sx={{
                              cursor: 'pointer',
                              '&:hover': {
                                bgcolor: 'action.hover',
                              },
                              bgcolor:
                                standing.position <= 2
                                  ? 'success.light'
                                  : standing.position === 3
                                  ? 'warning.light'
                                  : 'inherit',
                            }}
                            onClick={() => handleTeamClick(standing.teamId)}
                          >
                            <TableCell>
                              <Chip
                                label={standing.position}
                                size="small"
                                color={
                                  standing.position <= 2
                                    ? 'success'
                                    : standing.position === 3
                                    ? 'warning'
                                    : 'default'
                                }
                              />
                            </TableCell>
                            <TableCell>
                              <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                                <Avatar
                                  src={standing.teamFlagUrl}
                                  alt={standing.teamName}
                                  sx={{
                                    width: 32,
                                    height: 32,
                                    border: '1px solid',
                                    borderColor: 'divider'
                                  }}
                                  variant="rounded"
                                >
                                  {standing.teamCode?.substring(0, 2) || '??'}
                                </Avatar>
                                <Box>
                                  <Typography variant="body2" sx={{ fontWeight: 'medium' }}>
                                    {standing.teamName}
                                  </Typography>
                                  <Typography variant="caption" color="text.secondary">
                                    {standing.teamCode}
                                  </Typography>
                                </Box>
                              </Box>
                            </TableCell>
                            <TableCell align="center">{standing.played}</TableCell>
                            <TableCell align="center">{standing.won}</TableCell>
                            <TableCell align="center">{standing.drawn}</TableCell>
                            <TableCell align="center">{standing.lost}</TableCell>
                            <TableCell align="center">{standing.goalsFor}</TableCell>
                            <TableCell align="center">{standing.goalsAgainst}</TableCell>
                            <TableCell
                              align="center"
                              sx={{
                                color:
                                  standing.goalDifference > 0
                                    ? 'success.main'
                                    : standing.goalDifference < 0
                                    ? 'error.main'
                                    : 'inherit',
                                fontWeight: 'medium',
                              }}
                            >
                              {standing.goalDifference > 0 ? '+' : ''}
                              {standing.goalDifference}
                            </TableCell>
                            <TableCell align="center">
                              <Typography variant="body2" sx={{ fontWeight: 'bold' }}>
                                {standing.points}
                              </Typography>
                            </TableCell>
                          </TableRow>
                        ))}
                    </TableBody>
                  </Table>
                </TableContainer>
              </Paper>
            ))}
        </Box>
      ) : (
        <Paper sx={{ p: 4, textAlign: 'center' }}>
          <Typography variant="h6" color="text.secondary">
            No standings available
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
            Standings will be calculated once matches are played
          </Typography>
        </Paper>
      )}

      {/* Legend */}
      <Paper sx={{ p: 2, mt: 3 }}>
        <Typography variant="body2" sx={{ fontWeight: 'bold', mb: 1 }}>
          Legend:
        </Typography>
        <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
          <Chip label="Qualified for Round of 32" size="small" color="success" />
          <Chip label="Possible qualification" size="small" color="warning" />
          <Typography variant="caption" color="text.secondary">
            P = Played, W = Won, D = Drawn, L = Lost, GF = Goals For, GA = Goals Against, GD =
            Goal Difference, Pts = Points
          </Typography>
        </Box>
      </Paper>
    </Box>
  );
};

export default StandingsPage;

// Made with Bob
