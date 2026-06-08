import {
  Box,
  Card,
  CardContent,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Chip,
  IconButton,
  Avatar,
} from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { useParams, useNavigate } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { groupsService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';

const GroupDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const groupId = parseInt(id || '0');

  // Fetch group with standings
  const { data: groupData, isLoading, error } = useQuery({
    queryKey: ['group-with-standings', groupId],
    queryFn: () => groupsService.getWithStandings(groupId),
    enabled: !!groupId,
  });

  const handleTeamClick = (teamId: number) => {
    navigate(`/teams/${teamId}`);
  };

  if (error) {
    return (
      <ErrorMessage
        title="Failed to load group details"
        message={error instanceof Error ? error.message : 'An error occurred'}
      />
    );
  }

  if (isLoading) {
    return <Loading message="Loading group details..." />;
  }

  if (!groupData) {
    return (
      <ErrorMessage
        title="Group not found"
        message="The requested group could not be found."
      />
    );
  }

  const standings = groupData.standings || [];

  return (
    <Box>
      <PageHeader
        title={`Group ${groupData.name}`}
        subtitle={groupData.description || 'FIFA World Cup 2026 - Group Stage'}
        breadcrumbs={[
          { label: 'Home', path: '/' },
          { label: 'Groups', path: '/groups' },
          { label: `Group ${groupData.name}` },
        ]}
        action={
          <IconButton onClick={() => navigate('/groups')} color="primary">
            <ArrowBackIcon />
          </IconButton>
        }
      />

      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: { xs: '1fr', lg: '3fr 1fr' },
          gap: 3,
        }}
      >
        {/* Standings Table */}
        <Paper>
          <Box sx={{ p: 2, bgcolor: 'primary.main', color: 'white' }}>
            <Typography variant="h6">Group Standings</Typography>
          </Box>
          <TableContainer sx={{ overflowX: 'auto' }}>
            <Table sx={{ minWidth: 650 }}>
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
                {standings.length > 0 ? (
                  standings
                    .sort((a, b) => a.position - b.position)
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
                        <TableCell sx={{ minWidth: 200 }}>
                          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                            <Avatar
                              src={standing.teamFlagUrl}
                              alt={standing.teamName}
                              sx={{
                                width: 28,
                                height: 28,
                                border: '1px solid',
                                borderColor: 'divider'
                              }}
                              variant="rounded"
                            >
                              {standing.teamCode?.substring(0, 2) || '??'}
                            </Avatar>
                            <Box sx={{ minWidth: 0, flex: 1 }}>
                              <Typography variant="body2" sx={{ fontWeight: 'medium', whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' }}>
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
                    ))
                ) : (
                  <TableRow>
                    <TableCell colSpan={10} align="center">
                      <Typography color="text.secondary" sx={{ py: 4 }}>
                        No standings available yet. Standings will be calculated once matches are
                        played.
                      </Typography>
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>

        {/* Group Information */}
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Group Information
              </Typography>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 2 }}>
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Group Name
                  </Typography>
                  <Typography variant="h4" color="primary">
                    {groupData.name}
                  </Typography>
                </Box>
                {groupData.description && (
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      Description
                    </Typography>
                    <Typography variant="body1">{groupData.description}</Typography>
                  </Box>
                )}
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Teams
                  </Typography>
                  <Typography variant="body1">{standings.length} teams</Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>

          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Qualification
              </Typography>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5, mt: 2 }}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <Chip label="1-2" size="small" color="success" />
                  <Typography variant="body2">Qualify for Round of 32</Typography>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <Chip label="3" size="small" color="warning" />
                  <Typography variant="body2">Possible qualification (best 3rd place)</Typography>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <Chip label="4" size="small" color="default" />
                  <Typography variant="body2">Eliminated</Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>

          <Paper sx={{ p: 2, bgcolor: 'info.light' }}>
            <Typography variant="body2" sx={{ fontWeight: 'bold', mb: 1 }}>
              Legend:
            </Typography>
            <Typography variant="caption" color="text.secondary">
              P = Played, W = Won, D = Drawn, L = Lost, GF = Goals For, GA = Goals Against, GD =
              Goal Difference, Pts = Points
            </Typography>
          </Paper>
        </Box>
      </Box>
    </Box>
  );
};

export default GroupDetailsPage;

// Made with Bob
