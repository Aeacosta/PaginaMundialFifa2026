import { createBrowserRouter, Navigate } from 'react-router-dom';
import MainLayout from '../layouts/MainLayout';
import DashboardPage from '../pages/Dashboard/DashboardPage';
import TeamsPage from '../pages/Teams/TeamsPage';
import TeamDetailsPage from '../pages/Teams/TeamDetailsPage';
import GroupsPage from '../pages/Groups/GroupsPage';
import GroupDetailsPage from '../pages/Groups/GroupDetailsPage';
import MatchesPage from '../pages/Matches/MatchesPage';
import MatchDetailsPage from '../pages/Matches/MatchDetailsPage';
import StadiumsPage from '../pages/Stadiums/StadiumsPage';
import StadiumDetailsPage from '../pages/Stadiums/StadiumDetailsPage';
import StandingsPage from '../pages/Standings/StandingsPage';
import NotFoundPage from '../pages/NotFound/NotFoundPage';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <MainLayout />,
    children: [
      {
        index: true,
        element: <Navigate to="/dashboard" replace />,
      },
      {
        path: 'dashboard',
        element: <DashboardPage />,
      },
      {
        path: 'teams',
        children: [
          {
            index: true,
            element: <TeamsPage />,
          },
          {
            path: ':id',
            element: <TeamDetailsPage />,
          },
        ],
      },
      {
        path: 'groups',
        children: [
          {
            index: true,
            element: <GroupsPage />,
          },
          {
            path: ':id',
            element: <GroupDetailsPage />,
          },
        ],
      },
      {
        path: 'matches',
        children: [
          {
            index: true,
            element: <MatchesPage />,
          },
          {
            path: ':id',
            element: <MatchDetailsPage />,
          },
        ],
      },
      {
        path: 'stadiums',
        children: [
          {
            index: true,
            element: <StadiumsPage />,
          },
          {
            path: ':id',
            element: <StadiumDetailsPage />,
          },
        ],
      },
      {
        path: 'standings',
        element: <StandingsPage />,
      },
      {
        path: '*',
        element: <NotFoundPage />,
      },
    ],
  },
]);

// Made with Bob
