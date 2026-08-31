import { usePermission } from "./usePermission";
import { PERMISSIONS } from "../constants/permissions";

export function useIsManagement(): boolean {
    // 1. Call ALL hooks unconditionally in fixed order
    const canViewDepartments = usePermission(PERMISSIONS.DEPARTMENTS.VIEW);
    const canUpdateDepartments = usePermission(PERMISSIONS.DEPARTMENTS.UPDATE);
    const canCreateDepartments = usePermission(PERMISSIONS.DEPARTMENTS.CREATE);
    const canDeleteDepartments = usePermission(PERMISSIONS.DEPARTMENTS.DELETE);

    const canViewPositions = usePermission(PERMISSIONS.POSITIONS.VIEW);
    const canUpdatePositions = usePermission(PERMISSIONS.POSITIONS.UPDATE);
    const canCreatePositions = usePermission(PERMISSIONS.POSITIONS.CREATE);
    const canDeletePositions = usePermission(PERMISSIONS.POSITIONS.DELETE);

    const canViewEmployees = usePermission(PERMISSIONS.EMPLOYEES.VIEW);
    const canUpdateEmployees = usePermission(PERMISSIONS.EMPLOYEES.UPDATE);
    const canCreateEmployees = usePermission(PERMISSIONS.EMPLOYEES.CREATE);
    const canDeleteEmployees = usePermission(PERMISSIONS.EMPLOYEES.DELETE);
    const canViewSensitiveData = usePermission(PERMISSIONS.EMPLOYEES.ViewSensitiveData);

    const canViewAttendance = usePermission(PERMISSIONS.ATTENDANCE.VIEW);

    const canViewCorrections = usePermission(PERMISSIONS.ATTENDANCE_CORRECTIONS.VIEW);
    const canApproveCorrections = usePermission(PERMISSIONS.ATTENDANCE_CORRECTIONS.APPROVE_AND_REJECT);
    const canUpdateCorrections = usePermission(PERMISSIONS.ATTENDANCE_CORRECTIONS.UPDATE);
    const canDeleteCorrections = usePermission(PERMISSIONS.ATTENDANCE_CORRECTIONS.DELETE);

    const canViewLeaveRequests = usePermission(PERMISSIONS.LEAVE_REQUESTS.VIEW);
    const canApproveLeaveRequests = usePermission(PERMISSIONS.LEAVE_REQUESTS.APPROVE_AND_REJECT);

    const canCreateLeaveTypes = usePermission(PERMISSIONS.LEAVE_TYPES.CREATE);
    const canUpdateLeaveTypes = usePermission(PERMISSIONS.LEAVE_TYPES.UPDATE);
    const canViewLeaveTypes = usePermission(PERMISSIONS.LEAVE_TYPES.VIEW);

    const canCreateRoles = usePermission(PERMISSIONS.ROLES.CREATE);
    const canUpdateRoles = usePermission(PERMISSIONS.ROLES.UPDATE);
    const canViewRoles = usePermission(PERMISSIONS.ROLES.VIEW);

    const canViewPermissions = usePermission(PERMISSIONS.PERMISSIONS.VIEW);
    const canManageWorkSchedules = usePermission(PERMISSIONS.WORK_SCHEDULES.MANAGE);

    // 2. Return boolean OR evaluation of the results
    return (
        canViewDepartments ||
        canUpdateDepartments ||
        canCreateDepartments ||
        canDeleteDepartments ||
        canViewPositions ||
        canUpdatePositions ||
        canCreatePositions ||
        canDeletePositions ||
        canViewEmployees ||
        canUpdateEmployees ||
        canCreateEmployees ||
        canDeleteEmployees ||
        canViewSensitiveData ||
        canViewAttendance ||
        canViewCorrections ||
        canApproveCorrections ||
        canUpdateCorrections ||
        canDeleteCorrections ||
        canViewLeaveRequests ||
        canApproveLeaveRequests ||
        canCreateLeaveTypes ||
        canUpdateLeaveTypes ||
        canViewLeaveTypes ||
        canCreateRoles ||
        canUpdateRoles ||
        canViewRoles ||
        canViewPermissions ||
        canManageWorkSchedules
    );
}
