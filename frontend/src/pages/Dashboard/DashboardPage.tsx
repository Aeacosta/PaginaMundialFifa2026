import { Box, Typography, Paper } from '@mui/material';

const DashboardPage = () => {
  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ mb: 3, fontWeight: 600 }}>
        Dashboard
      </Typography>
      <Paper sx={{ p: 3 }}>
        <Typography variant="h6" gutterBottom>
          Welcome to FIFA World Cup 2026
        </Typography>
        <Typography variant="body1" color="text.secondary">
          Dashboard content will be implemented in Phase 6.4
        </Typography>
      </Paper>
    </Box>
  );
};

export default DashboardPage;

// Made with Bob
