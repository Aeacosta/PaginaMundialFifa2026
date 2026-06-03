import { Box, Typography, Paper } from '@mui/material';
import { useParams } from 'react-router-dom';

const GroupDetailsPage = () => {
  const { id } = useParams<{ id: string }>();

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ mb: 3, fontWeight: 600 }}>
        Group Details
      </Typography>
      <Paper sx={{ p: 3 }}>
        <Typography variant="body1" color="text.secondary">
          Group details for ID: {id} will be implemented in Phase 6.4
        </Typography>
      </Paper>
    </Box>
  );
};

export default GroupDetailsPage;

// Made with Bob
