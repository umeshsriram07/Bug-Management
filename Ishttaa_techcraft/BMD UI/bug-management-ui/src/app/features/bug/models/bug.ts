export interface Bug {
  id: number;
  title: string;
  description: string;
  status: string;
  priority: string;
  createdBy: number;
  assignedTo?: number;
  createdAt?: Date;
  updatedAt?: Date;
}
