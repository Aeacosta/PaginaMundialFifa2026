import { createTheme, alpha } from '@mui/material/styles';

// Professional Dark Theme inspired by Sneat and Material Kit
// FIFA World Cup 2026 color palette with modern dark mode
export const theme = createTheme({
  palette: {
    mode: 'dark',
    primary: {
      main: '#696cff',
      light: '#8b8dff',
      dark: '#5f61e6',
      contrastText: '#ffffff',
    },
    secondary: {
      main: '#8592a3',
      light: '#9ea8b5',
      dark: '#6f7d8f',
      contrastText: '#ffffff',
    },
    success: {
      main: '#56ca00',
      light: '#6fd943',
      dark: '#4caf50',
      contrastText: '#ffffff',
    },
    info: {
      main: '#16b1ff',
      light: '#42c3ff',
      dark: '#0d9de6',
      contrastText: '#ffffff',
    },
    warning: {
      main: '#ffb400',
      light: '#ffc233',
      dark: '#e6a200',
      contrastText: '#ffffff',
    },
    error: {
      main: '#ff4c51',
      light: '#ff6b6f',
      dark: '#e64449',
      contrastText: '#ffffff',
    },
    background: {
      default: '#232333',
      paper: '#2b2c40',
    },
    text: {
      primary: '#e7e7ff',
      secondary: '#a8aaae',
      disabled: '#6f7174',
    },
    divider: alpha('#e7e7ff', 0.12),
    action: {
      active: '#e7e7ff',
      hover: alpha('#e7e7ff', 0.08),
      selected: alpha('#696cff', 0.16),
      disabled: alpha('#e7e7ff', 0.3),
      disabledBackground: alpha('#e7e7ff', 0.12),
    },
  },
  typography: {
    fontFamily: [
      'Inter',
      'Roboto',
      '-apple-system',
      'BlinkMacSystemFont',
      '"Segoe UI"',
      'Arial',
      'sans-serif',
    ].join(','),
    h1: {
      fontSize: '2.5rem',
      fontWeight: 600,
      lineHeight: 1.2,
      letterSpacing: '-0.01562em',
    },
    h2: {
      fontSize: '2rem',
      fontWeight: 600,
      lineHeight: 1.3,
      letterSpacing: '-0.00833em',
    },
    h3: {
      fontSize: '1.75rem',
      fontWeight: 600,
      lineHeight: 1.4,
    },
    h4: {
      fontSize: '1.5rem',
      fontWeight: 500,
      lineHeight: 1.4,
    },
    h5: {
      fontSize: '1.25rem',
      fontWeight: 500,
      lineHeight: 1.5,
    },
    h6: {
      fontSize: '1rem',
      fontWeight: 600,
      lineHeight: 1.6,
    },
    button: {
      textTransform: 'none',
      fontWeight: 500,
      letterSpacing: '0.02857em',
    },
    body1: {
      fontSize: '0.9375rem',
      lineHeight: 1.5,
    },
    body2: {
      fontSize: '0.8125rem',
      lineHeight: 1.43,
    },
  },
  shape: {
    borderRadius: 6,
  },
  shadows: [
    'none',
    '0px 2px 4px rgba(15, 20, 34, 0.4)',
    '0px 4px 8px rgba(15, 20, 34, 0.4)',
    '0px 6px 12px rgba(15, 20, 34, 0.4)',
    '0px 8px 16px rgba(15, 20, 34, 0.4)',
    '0px 10px 20px rgba(15, 20, 34, 0.4)',
    '0px 12px 24px rgba(15, 20, 34, 0.4)',
    '0px 14px 28px rgba(15, 20, 34, 0.4)',
    '0px 16px 32px rgba(15, 20, 34, 0.4)',
    '0px 18px 36px rgba(15, 20, 34, 0.4)',
    '0px 20px 40px rgba(15, 20, 34, 0.4)',
    '0px 22px 44px rgba(15, 20, 34, 0.4)',
    '0px 24px 48px rgba(15, 20, 34, 0.4)',
    '0px 26px 52px rgba(15, 20, 34, 0.4)',
    '0px 28px 56px rgba(15, 20, 34, 0.4)',
    '0px 30px 60px rgba(15, 20, 34, 0.4)',
    '0px 32px 64px rgba(15, 20, 34, 0.4)',
    '0px 34px 68px rgba(15, 20, 34, 0.4)',
    '0px 36px 72px rgba(15, 20, 34, 0.4)',
    '0px 38px 76px rgba(15, 20, 34, 0.4)',
    '0px 40px 80px rgba(15, 20, 34, 0.4)',
    '0px 42px 84px rgba(15, 20, 34, 0.4)',
    '0px 44px 88px rgba(15, 20, 34, 0.4)',
    '0px 46px 92px rgba(15, 20, 34, 0.4)',
    '0px 48px 96px rgba(15, 20, 34, 0.4)',
  ],
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          scrollbarColor: '#6b6b6b #2b2c40',
          '&::-webkit-scrollbar, & *::-webkit-scrollbar': {
            width: 8,
            height: 8,
          },
          '&::-webkit-scrollbar-thumb, & *::-webkit-scrollbar-thumb': {
            borderRadius: 8,
            backgroundColor: '#6b6b6b',
            minHeight: 24,
          },
          '&::-webkit-scrollbar-thumb:hover, & *::-webkit-scrollbar-thumb:hover': {
            backgroundColor: '#8b8b8b',
          },
          '&::-webkit-scrollbar-track, & *::-webkit-scrollbar-track': {
            backgroundColor: '#2b2c40',
          },
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 6,
          padding: '8px 20px',
          fontSize: '0.9375rem',
          fontWeight: 500,
          transition: 'all 0.2s ease-in-out',
          '&:hover': {
            transform: 'translateY(-2px)',
          },
        },
        contained: {
          boxShadow: '0 2px 6px 0 rgba(105, 108, 255, 0.4)',
          '&:hover': {
            boxShadow: '0 4px 12px 0 rgba(105, 108, 255, 0.6)',
          },
        },
        outlined: {
          borderWidth: '1.5px',
          '&:hover': {
            borderWidth: '1.5px',
          },
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 8,
          boxShadow: '0 2px 6px 0 rgba(15, 20, 34, 0.4)',
          backgroundImage: 'none',
          transition: 'all 0.3s ease-in-out',
          '&:hover': {
            transform: 'translateY(-4px)',
            boxShadow: '0 6px 20px 0 rgba(15, 20, 34, 0.6)',
          },
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          backgroundImage: 'none',
          transition: 'all 0.3s ease-in-out',
        },
        elevation1: {
          boxShadow: '0 2px 6px 0 rgba(15, 20, 34, 0.4)',
        },
        elevation2: {
          boxShadow: '0 4px 8px 0 rgba(15, 20, 34, 0.4)',
        },
        elevation3: {
          boxShadow: '0 6px 12px 0 rgba(15, 20, 34, 0.4)',
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          boxShadow: '0 2px 6px 0 rgba(15, 20, 34, 0.4)',
          backgroundImage: 'none',
          backgroundColor: '#2b2c40',
        },
      },
    },
    MuiDrawer: {
      styleOverrides: {
        paper: {
          backgroundImage: 'none',
          backgroundColor: '#2b2c40',
          borderRight: '1px solid rgba(231, 231, 255, 0.12)',
        },
      },
    },
    MuiChip: {
      styleOverrides: {
        root: {
          borderRadius: 6,
          fontWeight: 500,
          transition: 'all 0.2s ease-in-out',
          '&:hover': {
            transform: 'scale(1.05)',
          },
        },
      },
    },
    MuiListItemButton: {
      styleOverrides: {
        root: {
          borderRadius: 6,
          margin: '4px 8px',
          transition: 'all 0.2s ease-in-out',
          '&:hover': {
            backgroundColor: alpha('#696cff', 0.08),
            transform: 'translateX(4px)',
          },
          '&.Mui-selected': {
            backgroundColor: alpha('#696cff', 0.16),
            '&:hover': {
              backgroundColor: alpha('#696cff', 0.24),
            },
          },
        },
      },
    },
    MuiTableCell: {
      styleOverrides: {
        root: {
          borderBottom: '1px solid rgba(231, 231, 255, 0.12)',
        },
        head: {
          fontWeight: 600,
          backgroundColor: alpha('#696cff', 0.08),
        },
      },
    },
    MuiTableRow: {
      styleOverrides: {
        root: {
          transition: 'all 0.2s ease-in-out',
          '&:hover': {
            backgroundColor: alpha('#696cff', 0.04),
          },
        },
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          '& .MuiOutlinedInput-root': {
            transition: 'all 0.2s ease-in-out',
            '&:hover': {
              '& .MuiOutlinedInput-notchedOutline': {
                borderColor: alpha('#696cff', 0.5),
              },
            },
            '&.Mui-focused': {
              '& .MuiOutlinedInput-notchedOutline': {
                borderWidth: '2px',
              },
            },
          },
        },
      },
    },
    MuiTooltip: {
      styleOverrides: {
        tooltip: {
          backgroundColor: '#2b2c40',
          border: '1px solid rgba(231, 231, 255, 0.12)',
          fontSize: '0.8125rem',
        },
        arrow: {
          color: '#2b2c40',
        },
      },
    },
    MuiAlert: {
      styleOverrides: {
        root: {
          borderRadius: 6,
        },
      },
    },
    MuiLinearProgress: {
      styleOverrides: {
        root: {
          borderRadius: 6,
          height: 6,
        },
      },
    },
    MuiCircularProgress: {
      styleOverrides: {
        root: {
          animationDuration: '0.8s',
        },
      },
    },
  },
});

// Made with Bob
