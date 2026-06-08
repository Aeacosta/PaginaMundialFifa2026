import { Box, Card, CardContent, CardMedia, Typography, Paper, IconButton, Chip } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { useParams, useNavigate } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import PeopleIcon from '@mui/icons-material/People';
import { stadiumsService } from '../../services/api';
import { PageHeader, Loading, ErrorMessage } from '../../components/shared';

const StadiumDetailsPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const stadiumId = parseInt(id || '0');

  // Fetch stadium details
  const { data: stadium, isLoading, error } = useQuery({
    queryKey: ['stadium', stadiumId],
    queryFn: () => stadiumsService.getById(stadiumId),
    enabled: !!stadiumId,
  });

  if (error) {
    return (
      <ErrorMessage
        title="Failed to load stadium details"
        message={error instanceof Error ? error.message : 'An error occurred'}
      />
    );
  }

  if (isLoading) {
    return <Loading message="Loading stadium details..." />;
  }

  if (!stadium) {
    return (
      <ErrorMessage
        title="Stadium not found"
        message="The requested stadium could not be found."
      />
    );
  }

  return (
    <Box>
      <PageHeader
        title={
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            {stadium.name}
            {stadium.flagUrl && (
              <Box
                component="img"
                src={stadium.flagUrl}
                alt={`${stadium.country} flag`}
                sx={{
                  width: 32,
                  height: 21,
                  objectFit: 'cover',
                  borderRadius: 0.5,
                }}
              />
            )}
          </Box>
        }
        subtitle={`${stadium.city}, ${stadium.country}`}
        breadcrumbs={[
          { label: 'Home', path: '/' },
          { label: 'Stadiums', path: '/stadiums' },
          { label: stadium.name },
        ]}
        action={
          <IconButton onClick={() => navigate('/stadiums')} color="primary">
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
        {/* Stadium Image and Info */}
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
          {stadium.imageUrl && (
            <Card>
              <CardMedia
                component="img"
                height="400"
                image={stadium.imageUrl}
                alt={stadium.name}
                sx={{ objectFit: 'cover' }}
              />
            </Card>
          )}

          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Stadium Information
              </Typography>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 2 }}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <LocationOnIcon color="action" />
                  <Box sx={{ flex: 1 }}>
                    <Typography variant="body2" color="text.secondary">
                      Location
                    </Typography>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                      <Typography variant="body1">
                        {stadium.city}, {stadium.country}
                      </Typography>
                      {stadium.flagUrl && (
                        <Box
                          component="img"
                          src={stadium.flagUrl}
                          alt={`${stadium.country} flag`}
                          sx={{
                            width: 28,
                            height: 19,
                            objectFit: 'cover',
                            borderRadius: 0.5,
                          }}
                        />
                      )}
                    </Box>
                  </Box>
                </Box>

                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <PeopleIcon color="action" />
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      Capacity
                    </Typography>
                    <Typography variant="body1">
                      {stadium.capacity.toLocaleString()} spectators
                    </Typography>
                  </Box>
                </Box>

                {(stadium.latitude && stadium.longitude) && (
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      Coordinates
                    </Typography>
                    <Typography variant="body1">
                      {stadium.latitude.toFixed(6)}, {stadium.longitude.toFixed(6)}
                    </Typography>
                  </Box>
                )}
              </Box>
            </CardContent>
          </Card>
        </Box>

        {/* Stadium Stats */}
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Quick Facts
              </Typography>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 2 }}>
                <Paper variant="outlined" sx={{ p: 2, textAlign: 'center' }}>
                  <Typography variant="h3" color="primary">
                    {stadium.capacity.toLocaleString()}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Seating Capacity
                  </Typography>
                </Paper>

                <Box>
                  <Typography variant="body2" color="text.secondary" gutterBottom>
                    Host Country
                  </Typography>
                  <Chip
                    label={stadium.country}
                    color="primary"
                    sx={{ width: '100%' }}
                  />
                </Box>

                <Box>
                  <Typography variant="body2" color="text.secondary" gutterBottom>
                    City
                  </Typography>
                  <Chip
                    label={stadium.city}
                    color="secondary"
                    sx={{ width: '100%' }}
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
              This stadium is one of the 16 venues selected to host matches during the FIFA World
              Cup 2026, which will be jointly hosted by the United States, Mexico, and Canada.
            </Typography>
          </Paper>
        </Box>
      </Box>
    </Box>
  );
};

export default StadiumDetailsPage;

// Made with Bob
