import { useState } from 'react';
import {
  Box,
  Card,
  CardContent,
  CardMedia,
  Chip,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
  Typography,
  InputAdornment,
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { teamsService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';
import { Confederation } from '../../types';

const TeamsPage = () => {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedConfederation, setSelectedConfederation] = useState<string>('');
  const [selectedGroup, setSelectedGroup] = useState<string>('');

  // Fetch all teams
  const { data: teams, isLoading, error, refetch } = useQuery({
    queryKey: ['teams', searchTerm, selectedConfederation, selectedGroup],
    queryFn: async () => {
      if (searchTerm) {
        return teamsService.search(searchTerm);
      }
      if (selectedConfederation) {
        return teamsService.getByConfederation(selectedConfederation as Confederation);
      }
      if (selectedGroup) {
        const groupId = selectedGroup.charCodeAt(0) - 64; // Convert 'A' to 1, 'B' to 2, etc.
        return teamsService.getByGroup(groupId);
      }
      const result = await teamsService.getAll(1, 100); // Get all teams
      return result.items;
    },
  });

  const handleTeamClick = (teamId: number) => {
    navigate(`/teams/${teamId}`);
  };

  const handleClearFilters = () => {
    setSearchTerm('');
    setSelectedConfederation('');
    setSelectedGroup('');
  };

  if (error) {
    return (
      <ErrorMessage
        title="Failed to load teams"
        message={error instanceof Error ? error.message : 'An error occurred'}
        onRetry={refetch}
      />
    );
  }

  const confederations = Object.values(Confederation);
  const groups = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L'];

  return (
    <Box>
      <PageHeader
        title="Teams"
        subtitle={`${teams?.length || 48} teams qualified for FIFA World Cup 2026`}
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
        <TextField
          placeholder="Search teams..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          sx={{ flexGrow: 1, minWidth: 250 }}
          slotProps={{
            input: {
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon />
                </InputAdornment>
              ),
            },
          }}
        />

        <FormControl sx={{ minWidth: 200 }}>
          <InputLabel>Confederation</InputLabel>
          <Select
            value={selectedConfederation}
            label="Confederation"
            onChange={(e) => setSelectedConfederation(e.target.value)}
          >
            <MenuItem value="">All Confederations</MenuItem>
            {confederations.map((conf) => (
              <MenuItem key={conf} value={conf}>
                {conf}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <FormControl sx={{ minWidth: 150 }}>
          <InputLabel>Group</InputLabel>
          <Select
            value={selectedGroup}
            label="Group"
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

        {(searchTerm || selectedConfederation || selectedGroup) && (
          <Chip
            label="Clear Filters"
            onDelete={handleClearFilters}
            color="primary"
            variant="outlined"
          />
        )}
      </Box>

      {/* Teams Grid */}
      {isLoading ? (
        <Loading message="Loading teams..." />
      ) : teams && teams.length > 0 ? (
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: {
              xs: '1fr',
              sm: 'repeat(2, 1fr)',
              md: 'repeat(3, 1fr)',
              lg: 'repeat(4, 1fr)',
            },
            gap: 3,
          }}
        >
          {teams.map((team) => (
            <Card
              key={team.id}
              sx={{
                cursor: 'pointer',
                transition: 'transform 0.2s, box-shadow 0.2s',
                '&:hover': {
                  transform: 'translateY(-4px)',
                  boxShadow: 4,
                },
              }}
              onClick={() => handleTeamClick(team.id)}
            >
              {team.flagUrl && (
                <CardMedia
                  component="img"
                  height="140"
                  image={team.flagUrl}
                  alt={`${team.name} flag`}
                  sx={{ objectFit: 'cover' }}
                />
              )}
              <CardContent>
                <Typography variant="h6" component="div" gutterBottom>
                  {team.name}
                </Typography>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  {team.code}
                </Typography>
                <Box sx={{ display: 'flex', gap: 1, mt: 2, flexWrap: 'wrap' }}>
                  <Chip
                    label={team.confederation}
                    size="small"
                    color="primary"
                    variant="outlined"
                  />
                  {team.groupName && (
                    <Chip
                      label={`Group ${team.groupName}`}
                      size="small"
                      color="secondary"
                      variant="outlined"
                    />
                  )}
                  {team.fifaRanking && (
                    <Chip
                      label={`FIFA #${team.fifaRanking}`}
                      size="small"
                      variant="outlined"
                    />
                  )}
                </Box>
              </CardContent>
            </Card>
          ))}
        </Box>
      ) : (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h6" color="text.secondary">
            No teams found
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
            Try adjusting your search or filters
          </Typography>
        </Box>
      )}
    </Box>
  );
};

export default TeamsPage;

// Made with Bob
