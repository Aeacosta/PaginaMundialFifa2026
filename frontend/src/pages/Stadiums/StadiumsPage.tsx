import { useState } from 'react';
import {
  Box,
  Card,
  CardContent,
  CardMedia,
  Typography,
  TextField,
  InputAdornment,
  Chip,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { stadiumsService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';

const StadiumsPage = () => {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCountry, setSelectedCountry] = useState<string>('');

  // Fetch stadiums
  const { data: stadiums, isLoading, error, refetch } = useQuery({
    queryKey: ['stadiums', searchTerm, selectedCountry],
    queryFn: async () => {
      if (searchTerm) {
        return stadiumsService.search(searchTerm);
      }
      if (selectedCountry) {
        return stadiumsService.getByCountry(selectedCountry);
      }
      const result = await stadiumsService.getAll({ page: 1, pageSize: 100 });
      return result.items;
    },
  });

  const handleStadiumClick = (stadiumId: number) => {
    navigate(`/stadiums/${stadiumId}`);
  };

  const handleClearFilters = () => {
    setSearchTerm('');
    setSelectedCountry('');
  };

  if (error) {
    return (
      <ErrorMessage
        title="Failed to load stadiums"
        message={error instanceof Error ? error.message : 'An error occurred'}
        onRetry={refetch}
      />
    );
  }

  const countries = ['United States', 'Mexico', 'Canada'];

  return (
    <Box>
      <PageHeader
        title="Stadiums"
        subtitle={`${stadiums?.length || 16} stadiums hosting FIFA World Cup 2026`}
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
          placeholder="Search stadiums..."
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
          <InputLabel>Country</InputLabel>
          <Select
            value={selectedCountry}
            label="Country"
            onChange={(e) => setSelectedCountry(e.target.value)}
          >
            <MenuItem value="">All Countries</MenuItem>
            {countries.map((country) => (
              <MenuItem key={country} value={country}>
                {country}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        {(searchTerm || selectedCountry) && (
          <Chip
            label="Clear Filters"
            onDelete={handleClearFilters}
            color="primary"
            variant="outlined"
          />
        )}
      </Box>

      {/* Stadiums Grid */}
      {isLoading ? (
        <Loading message="Loading stadiums..." />
      ) : stadiums && stadiums.length > 0 ? (
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: {
              xs: '1fr',
              sm: 'repeat(2, 1fr)',
              md: 'repeat(3, 1fr)',
            },
            gap: 3,
          }}
        >
          {stadiums.map((stadium) => (
            <Card
              key={stadium.id}
              sx={{
                cursor: 'pointer',
                transition: 'transform 0.2s, box-shadow 0.2s',
                '&:hover': {
                  transform: 'translateY(-4px)',
                  boxShadow: 4,
                },
              }}
              onClick={() => handleStadiumClick(stadium.id)}
            >
              {stadium.imageUrl && (
                <CardMedia
                  component="img"
                  height="200"
                  image={stadium.imageUrl}
                  alt={stadium.name}
                  sx={{ objectFit: 'cover' }}
                />
              )}
              <CardContent>
                <Typography variant="h6" component="div" gutterBottom>
                  {stadium.name}
                </Typography>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5, mb: 1 }}>
                  <LocationOnIcon fontSize="small" color="action" />
                  <Typography variant="body2" color="text.secondary">
                    {stadium.city}, {stadium.country}
                  </Typography>
                </Box>
                <Box sx={{ display: 'flex', gap: 1, mt: 2, flexWrap: 'wrap' }}>
                  <Chip
                    label={`Capacity: ${stadium.capacity.toLocaleString()}`}
                    size="small"
                    color="primary"
                    variant="outlined"
                  />
                </Box>
              </CardContent>
            </Card>
          ))}
        </Box>
      ) : (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h6" color="text.secondary">
            No stadiums found
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
            Try adjusting your search or filters
          </Typography>
        </Box>
      )}
    </Box>
  );
};

export default StadiumsPage;

// Made with Bob
