export interface Department {
    id: number;
    name: string;
    code: string;
    description: string | null;
    managerName: string | null;
    managerEmployeeId: number | null;
}