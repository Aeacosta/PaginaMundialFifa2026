import { Box, Card, CardContent, Typography, Chip, Avatar } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { groupsService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';

const GroupsPage = () => {
  const navigate = useNavigate();

  // Fetch all groups with standings
  const { data: groups, isLoading, error, refetch } = useQuery({
    queryKey: ['groups-with-standings'],
    queryFn: groupsService.getAllWithStandings,
  });

  const handleGroupClick = (groupId: number) => {
    navigate(`/groups/${groupId}`);
  };

  if (error) {
    return (
      <ErrorMessage
        title="Failed to load groups"
        message={error instanceof Error ? error.message : 'An error occurred'}
        onRetry={refetch}
      />
    );
  }

  return (
    <Box>
      <PageHeader
        title="Groups"
        subtitle="FIFA World Cup 2026 - Group Stage Overview"
      />

      {isLoading ? (
        <Loading message="Loading groups..." />
      ) : groups && groups.length > 0 ? (
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: {
              xs: '1fr',
              sm: 'repeat(2, 1fr)',
              lg: 'repeat(3, 1fr)',
            },
            gap: 3,
          }}
        >
          {groups.map((group) => {
            const standings = group.standings || [];
            const topTeams = standings.slice(0, 2);

            return (
              <Card
                key={group.id}
                sx={{
                  cursor: 'pointer',
                  transition: 'transform 0.2s, box-shadow 0.2s',
                  '&:hover': {
                    transform: 'translateY(-4px)',
                    boxShadow: 4,
                  },
                }}
                onClick={() => handleGroupClick(group.id)}
              >
                <Box
                  sx={{
                    p: 2,
                    bgcolor: 'primary.main',
                    color: 'white',
                    textAlign: 'center',
                  }}
                >
                  <Typography variant="h5" sx={{ fontWeight: 'bold' }}>
                    Group {group.name}
                  </Typography>
                  {group.description && (
                    <Typography variant="caption">{group.description}</Typography>
                  )}
                </Box>

                <CardContent>
                  <Typography variant="subtitle2" color="text.secondary" gutterBottom>
                    Current Standings
                  </Typography>

                  {standings.length > 0 ? (
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5, mt: 2 }}>
                      {standings.map((standing, index) => (
                        <Box
                          key={standing.id}
                          sx={{
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'space-between',
                            p: 1,
                            borderRadius: 1,
                            bgcolor: index < 2 ? 'success.light' : 'background.default',
                          }}
                        >
                          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flex: 1, minWidth: 0 }}>
                            <Chip
                              label={standing.position}
                              size="small"
                              color={index < 2 ? 'success' : 'default'}
                              sx={{ minWidth: 32, flexShrink: 0 }}
                            />
                            <Avatar
                              src={standing.teamFlagUrl}
                              alt={standing.teamName}
                              sx={{
                                width: 24,
                                height: 24,
                                border: '1px solid',
                                borderColor: 'divider',
                                flexShrink: 0
                              }}
                              variant="rounded"
                            >
                              {standing.teamCode?.substring(0, 2) || '??'}
                            </Avatar>
                            <Box sx={{ minWidth: 0, flex: 1 }}>
                              <Typography variant="body2" sx={{ fontWeight: 'medium', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                                {standing.teamName}
                              </Typography>
                              <Typography variant="caption" color="text.secondary">
                                {standing.teamCode}
                              </Typography>
                            </Box>
                          </Box>
                          <Box sx={{ textAlign: 'right', flexShrink: 0, minWidth: 80 }}>
                            <Typography variant="body2" sx={{ fontWeight: 'bold', whiteSpace: 'nowrap' }}>
                              {standing.points} pts
                            </Typography>
                            <Typography variant="caption" color="text.secondary" sx={{ whiteSpace: 'nowrap' }}>
                              {standing.played}P {standing.won}W {standing.drawn}D {standing.lost}L
                            </Typography>
                          </Box>
                        </Box>
                      ))}
                    </Box>
                  ) : (
                    <Typography variant="body2" color="text.secondary" sx={{ mt: 2 }}>
                      No matches played yet
                    </Typography>
                  )}

                  {topTeams.length === 2 && (
                    <Box sx={{ mt: 2, pt: 2, borderTop: 1, borderColor: 'divider' }}>
                      <Typography variant="caption" color="text.secondary">
                        Top 2 teams qualify for Round of 32
                      </Typography>
                    </Box>
                  )}
                </CardContent>
              </Card>
            );
          })}
        </Box>
      ) : (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h6" color="text.secondary">
            No groups found
          </Typography>
        </Box>
      )}
    </Box>
  );
};

export default GroupsPage;

// Made with Bob
