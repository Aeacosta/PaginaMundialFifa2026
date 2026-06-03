import { Box, Typography, Paper } from '@mui/material';
import { useParams } from 'react-router-dom';

const MatchDetailsPage = () => {
  const { id } = useParams<{ id: string }>();

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ mb: 3, fontWeight: 600 }}>
        Match Details
      </Typography>
      <Paper sx={{ p: 3 }}>
        <Typography variant="body1" color="text.secondary">
          Match details for ID: {id} will be implemented in Phase 6.4
        </Typography>
      </Paper>
    </Box>
  );
};

export default MatchDetailsPage;

// Made with Bob
