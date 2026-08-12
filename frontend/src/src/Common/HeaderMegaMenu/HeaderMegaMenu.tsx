import {
  Avatar,
  Box,
  Burger,
  Button,
  Divider,
  Drawer,
  Group,
  Menu,
  Stack,
  Text,
  UnstyledButton,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { IconBuildingCommunity, IconLogout, IconUser } from "@tabler/icons-react";
import { Link, useNavigate } from "react-router-dom";

import { apiClient } from "../../lib/apiClient";
import { API_ROUTES } from "../../lib/apiRoutes";
import { useAuthStore } from "../../store/useAuthStore";
import { useEmployeeOptionsStore } from "../../features/Employees/store/useEmployeeOptionsStore";
import classes from "./HeaderMegaMenu.module.css";
import { PERMISSIONS } from "../../features/Auth/constants/permissions";
import { usePermission } from "../../features/Auth/hooks/usePermission";

export function HeaderMegaMenu() {
  const [drawerOpened, drawer] = useDisclosure(false);
  const navigate = useNavigate();

  const user = useAuthStore((state) => state.user);
  const isAuthenticated = useAuthStore((state) => Boolean(state.accessToken));
  const clearSession = useAuthStore((state) => state.clearSession);
  const invalidateEmployees = useEmployeeOptionsStore((state) => state.invalidate);

  const canViewDepartments = usePermission(PERMISSIONS.DEPARTMENTS.VIEW);
  const canViewEmployees = usePermission(PERMISSIONS.EMPLOYEES.VIEW);

  const displayName = user
    ? `${user.firstName} ${user.lastName}`.trim()
    : "";

  const initials = user
    ? `${user.firstName[0] ?? ""}${user.lastName[0] ?? ""}`.toUpperCase()
    : "";

  const handleLogout = async () => {
    try {
      await apiClient.post(API_ROUTES.AUTH.LOGOUT);
    } finally {
      invalidateEmployees();
      clearSession();
      drawer.close();
      navigate("/login", { replace: true });
    }
  };

  return (
    <Box>
      <header className={classes.header}>
        <Group justify="space-between" h="100%">
          <Link to={isAuthenticated ? "/departments" : "/login"} className={classes.brand}>
            HRMS
          </Link>

          {isAuthenticated && (
            <Group h="100%" gap={0} visibleFrom="sm">
              <Link to="/roles" className={classes.link}>
                Roles
              </Link>

              {canViewDepartments && <Link to="/departments" className={classes.link}>
                Departments
              </Link>}
              
              {canViewEmployees && <Link to="/employees" className={classes.link}>
                Employees
              </Link> }

            <Link to="/work-schedules" className={classes.link}>
                Work Schedules
              </Link>

            </Group>
          )}

          <Group visibleFrom="sm">
            {isAuthenticated && user ? (
              <Menu position="bottom-end" shadow="md" width={220}>
                <Menu.Target>
                  <UnstyledButton>
                    <Group gap="sm">
                      <Avatar color="indigo" radius="xl">
                        {initials}
                      </Avatar>
                      <div>
                        <Text c="white" size="sm" fw={600}>
                          {displayName}
                        </Text>
                        <Text c="gray.3" size="xs">
                          {user.email}
                        </Text>
                      </div>
                    </Group>
                  </UnstyledButton>
                </Menu.Target>

                <Menu.Dropdown>
                  <Menu.Label>Account</Menu.Label>
                  <Menu.Item leftSection={<IconUser size={16} />}>
                    Profile
                  </Menu.Item>
                  <Menu.Divider />
                  <Menu.Item
                    color="red"
                    leftSection={<IconLogout size={16} />}
                    onClick={handleLogout}
                  >
                    Log out
                  </Menu.Item>
                </Menu.Dropdown>
              </Menu>
            ) : (
              <>
                <Button component={Link} to="/login" variant="transparent" c="white">
                  Log in
                </Button>
                <Button component={Link} to="/register" variant="white" color="indigo">
                  Sign up
                </Button>
              </>
            )}
          </Group>

          <Burger
            opened={drawerOpened}
            onClick={drawer.toggle}
            hiddenFrom="sm"
            aria-label="Toggle navigation"
            color="white"
          />
        </Group>
      </header>

      <Drawer
        opened={drawerOpened}
        onClose={drawer.close}
        title="Navigation"
        hiddenFrom="sm"
        position="right"
      >
        <Stack>
          {isAuthenticated && user ? (
            <>
              <Group>
                <Avatar color="indigo" radius="xl">
                  {initials}
                </Avatar>
                <div>
                  <Text fw={600}>{displayName}</Text>
                  <Text size="xs" c="dimmed">{user.email}</Text>
                </div>
              </Group>
              <Divider />
              <Button
                component={Link}
                to="/departments"
                variant="subtle"
                leftSection={<IconBuildingCommunity size={17} />}
                onClick={drawer.close}
              >
                Departments
              </Button>
              <Button
                color="red"
                variant="light"
                leftSection={<IconLogout size={17} />}
                onClick={handleLogout}
              >
                Log out
              </Button>
            </>
          ) : (
            <>
              <Button component={Link} to="/login" onClick={drawer.close}>
                Log in
              </Button>
              <Button component={Link} to="/register" variant="light" onClick={drawer.close}>
                Sign up
              </Button>
            </>
          )}
        </Stack>
      </Drawer>
    </Box>
  );
}
