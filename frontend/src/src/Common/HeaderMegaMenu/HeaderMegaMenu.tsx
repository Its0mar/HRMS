import {
  ActionIcon,
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
  useComputedColorScheme,
  useMantineColorScheme,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import {
  IconBuildingCommunity,
  IconCalendarEvent,
  IconChevronDown,
  IconClock,
  IconClockCheck,
  IconFileCheck,
  IconKey,
  IconLogout,
  IconMoon,
  IconShieldCheck,
  IconSun,
  IconUser,
  IconUsers,
} from "@tabler/icons-react";
import { Link, useNavigate } from "react-router-dom";

import { apiClient } from "../../lib/apiClient";
import { API_ROUTES } from "../../lib/apiRoutes";
import { useAuthStore } from "../../store/useAuthStore";
import { useEmployeeOptionsStore } from "../../features/Employees/store/useEmployeeOptionsStore";
import classes from "./HeaderMegaMenu.module.css";
import { PERMISSIONS } from "../../features/Auth/constants/permissions";
import { usePermission } from "../../features/Auth/hooks/usePermission";
import { useIsManagement } from "../../features/Auth/hooks/useIsManagement";
import { ChangePasswordModal } from "../../features/Auth/components/ChangePasswordModal";

export function HeaderMegaMenu() {
  const [drawerOpened, drawer] = useDisclosure(false);
  const [changePasswordOpened, changePasswordModal] = useDisclosure(false);
  const navigate = useNavigate();

  const { setColorScheme } = useMantineColorScheme();
  const computedColorScheme = useComputedColorScheme("dark", { getInitialValueInEffect: true });

  const user = useAuthStore((state) => state.user);
  const isAuthenticated = useAuthStore((state) => Boolean(state.accessToken));
  const clearSession = useAuthStore((state) => state.clearSession);
  const invalidateEmployees = useEmployeeOptionsStore((state) => state.invalidate);

  // Global Management Check
  const isManagementUser = useIsManagement();

  // Fine-grained Permissions
  const canViewRoles = usePermission(PERMISSIONS.ROLES.VIEW);
  const canViewDepartments = usePermission(PERMISSIONS.DEPARTMENTS.VIEW);
  const canViewEmployees = usePermission(PERMISSIONS.EMPLOYEES.VIEW);
  const canViewWorkSchedules = usePermission(PERMISSIONS.WORK_SCHEDULES.MANAGE);
  const canViewCompanyAttendance = usePermission(PERMISSIONS.ATTENDANCE.VIEW);
  const canViewAttendanceCorrections = usePermission(PERMISSIONS.ATTENDANCE_CORRECTIONS.VIEW);
  const canViewLeaveTypes = usePermission(PERMISSIONS.LEAVE_TYPES.VIEW);
  const canViewCompanyLeaves = usePermission(PERMISSIONS.LEAVE_REQUESTS.VIEW);

  const displayName = user ? `${user.firstName} ${user.lastName}`.trim() : "";
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
          <Link to={isAuthenticated ? "/dashboard" : "/login"} className={classes.brand}>
            HRMS
          </Link>

          {isAuthenticated && (
            <Group h="100%" gap="sm" visibleFrom="sm">
              {/* Employee Links (Hidden for Management) */}
              {!isManagementUser && (
                <>
                  <Link to="/attendances" className={classes.link}>
                    My Attendance
                  </Link>

                  <Link to="/leaves/my" className={classes.link}>
                    My Leaves
                  </Link>
                </>
              )}

              {/* 1. Organization Dropdown Menu */}
              {(canViewEmployees || canViewDepartments || canViewRoles) && (
                <Menu position="bottom-start" shadow="md" width={220}>
                  <Menu.Target>
                    <UnstyledButton className={classes.link}>
                      <Group gap={4}>
                        <span>Organization</span>
                        <IconChevronDown size={14} />
                      </Group>
                    </UnstyledButton>
                  </Menu.Target>
                  <Menu.Dropdown>
                    {canViewEmployees && (
                      <Menu.Item
                        component={Link}
                        to="/employees"
                        leftSection={<IconUsers size={16} />}
                      >
                        Employees
                      </Menu.Item>
                    )}
                    {canViewDepartments && (
                      <Menu.Item
                        component={Link}
                        to="/departments"
                        leftSection={<IconBuildingCommunity size={16} />}
                      >
                        Departments
                      </Menu.Item>
                    )}
                    {canViewRoles && (
                      <Menu.Item
                        component={Link}
                        to="/roles"
                        leftSection={<IconShieldCheck size={16} />}
                      >
                        Roles & Permissions
                      </Menu.Item>
                    )}
                  </Menu.Dropdown>
                </Menu>
              )}

              {/* 2. Attendance & Shifts Dropdown Menu */}
              {(canViewCompanyAttendance || canViewAttendanceCorrections || canViewWorkSchedules) && (
                <Menu position="bottom-start" shadow="md" width={230}>
                  <Menu.Target>
                    <UnstyledButton className={classes.link}>
                      <Group gap={4}>
                        <span>Attendance & Shifts</span>
                        <IconChevronDown size={14} />
                      </Group>
                    </UnstyledButton>
                  </Menu.Target>
                  <Menu.Dropdown>
                    {canViewCompanyAttendance && (
                      <Menu.Item
                        component={Link}
                        to="/attendances/company"
                        leftSection={<IconClock size={16} />}
                      >
                        Company Attendance
                      </Menu.Item>
                    )}
                    {canViewAttendanceCorrections && (
                      <Menu.Item
                        component={Link}
                        to="/attendances/corrections/company"
                        leftSection={<IconClockCheck size={16} />}
                      >
                        Attendance Corrections
                      </Menu.Item>
                    )}
                    {canViewWorkSchedules && (
                      <Menu.Item
                        component={Link}
                        to="/work-schedules"
                        leftSection={<IconCalendarEvent size={16} />}
                      >
                        Work Schedules
                      </Menu.Item>
                    )}
                  </Menu.Dropdown>
                </Menu>
              )}

              {/* 3. Leaves & Vacations Dropdown Menu */}
              {(canViewCompanyLeaves || canViewLeaveTypes) && (
                <Menu position="bottom-start" shadow="md" width={220}>
                  <Menu.Target>
                    <UnstyledButton className={classes.link}>
                      <Group gap={4}>
                        <span>Leaves & Vacations</span>
                        <IconChevronDown size={14} />
                      </Group>
                    </UnstyledButton>
                  </Menu.Target>
                  <Menu.Dropdown>
                    {canViewCompanyLeaves && (
                      <Menu.Item
                        component={Link}
                        to="/leaves/company"
                        leftSection={<IconFileCheck size={16} />}
                      >
                        Leave Approvals
                      </Menu.Item>
                    )}
                    {canViewLeaveTypes && (
                      <Menu.Item
                        component={Link}
                        to="/leaves/types"
                        leftSection={<IconCalendarEvent size={16} />}
                      >
                        Leave Types Config
                      </Menu.Item>
                    )}
                  </Menu.Dropdown>
                </Menu>
              )}
            </Group>
          )}

          {/* Account Menu & Theme Toggle */}
          <Group gap="sm">
            <ActionIcon
              onClick={() => setColorScheme(computedColorScheme === "light" ? "dark" : "light")}
              variant="subtle"
              color="gray"
              size="lg"
              aria-label="Toggle color scheme"
            >
              {computedColorScheme === "dark" ? <IconSun size={20} color="#f59e0b" /> : <IconMoon size={20} color="#38bdf8" />}
            </ActionIcon>

            {isAuthenticated && user ? (
              <Box visibleFrom="sm">
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
                  <Menu.Item leftSection={<IconKey size={16} />} onClick={changePasswordModal.open}>
                    Change Password
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
              </Box>
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

      {/* Mobile Drawer */}
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

              {!isManagementUser && (
                <>
                  <Button component={Link} to="/leaves/my" variant="subtle" onClick={drawer.close}>
                    My Leaves
                  </Button>
                  <Button component={Link} to="/attendances" variant="subtle" onClick={drawer.close}>
                    My Attendance
                  </Button>
                </>
              )}

              {canViewEmployees && (
                <Button component={Link} to="/employees" variant="subtle" onClick={drawer.close}>
                  Employees
                </Button>
              )}
              {canViewCompanyAttendance && (
                <Button component={Link} to="/attendances/company" variant="subtle" onClick={drawer.close}>
                  Company Attendance
                </Button>
              )}
              {canViewAttendanceCorrections && (
                <Button component={Link} to="/attendances/corrections/company" variant="subtle" onClick={drawer.close}>
                  Attendance Corrections
                </Button>
              )}
              {canViewCompanyLeaves && (
                <Button component={Link} to="/leaves/company" variant="subtle" onClick={drawer.close}>
                  Leave Approvals
                </Button>
              )}
              {canViewLeaveTypes && (
                <Button component={Link} to="/leaves/types" variant="subtle" onClick={drawer.close}>
                  Leave Types Config
                </Button>
              )}

              <Divider />
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
      {/* Change Password Modal */}
      <ChangePasswordModal
        opened={changePasswordOpened}
        onClose={changePasswordModal.close}
      />
    </Box>
  );
}
