import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BugService } from '../../services/bug';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  bugs: any[] = [];
  filteredBugs: any[] = [];
  selectedStatus: string = 'All Status';
  showModal = false;
  submitted = false;
  isEditMode = false;
  editingBugId = 0;

  newBug: any = {
    title: '',
    description: '',
    status: 'Open',
    priority: 'Medium',
    createdBy: 1,
    assignedTo: 1,
  };

  openCount = 0;
  closedCount = 0;
  wipCount = 0;
  rejectedCount = 0;

  constructor(
    private bugService: BugService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadBugs();
  }

  loadBugs(): void {
    this.bugService.getAllBugs().subscribe({
      next: (response) => {
        const data = response?.data || [];
        this.bugs = [...data];

        this.applyFilter(this.selectedStatus);
        this.calculateCounts();

        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  calculateCounts(): void {
    this.openCount = this.bugs.filter((x) => x.status === 'Open').length;
    this.closedCount = this.bugs.filter((x) => x.status === 'Closed').length;
    this.wipCount = this.bugs.filter((x) => x.status === 'Work In Progress').length;
    this.rejectedCount = this.bugs.filter((x) => x.status === 'Rejected').length;
  }

  applyFilter(status: string): void {
    this.selectedStatus = status;
    if (status === 'All Status') {
      this.filteredBugs = [...this.bugs];
    } else {
      this.filteredBugs = this.bugs.filter((x) => x.status === status);
    }
  }

  filterByStatus(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.applyFilter(value);
  }

  deleteBug(id: number): void {
    const confirmDelete = confirm('Are you sure you want to delete this bug?');
    if (!confirmDelete) return;

    this.bugService.deleteBug(id).subscribe({
      next: () => {
        this.bugs = this.bugs.filter((x) => x.id !== id);
        this.applyFilter(this.selectedStatus);
        this.calculateCounts();
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  openModal(): void {
    this.resetForm();
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.resetForm();
    this.cdr.detectChanges();
  }

  resetForm(): void {
    this.submitted = false;
    this.isEditMode = false;
    this.editingBugId = 0;
    this.newBug = {
      title: '',
      description: '',
      status: 'Open',
      priority: 'Medium',
      createdBy: 1,
      assignedTo: 1,
    };
  }

  createBug(): void {
    this.submitted = true;

    if (!this.newBug.title?.trim() || !this.newBug.description?.trim()) {
      return;
    }

    if (this.isEditMode) {
      this.bugService.updateBug(this.editingBugId, this.newBug).subscribe({
        next: () => {
          this.showModal = false;
          this.resetForm();
          this.cdr.detectChanges();
          this.loadBugs();
        },
        error: (error) => {
          console.error(error);
        },
      });
      return;
    }

    this.bugService.createBug(this.newBug).subscribe({
      next: () => {
        this.showModal = false;
        this.resetForm();
        this.cdr.detectChanges();
        this.loadBugs();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  editBug(bug: any): void {
    this.isEditMode = true;
    this.editingBugId = bug.id;
    this.newBug = {
      title: bug.title,
      description: bug.description,
      status: bug.status,
      priority: bug.priority,
      createdBy: bug.createdBy,
      assignedTo: bug.assignedTo,
    };
    this.showModal = true;
    this.cdr.detectChanges();
  }

  trackById(index: number, bug: any): number {
    return bug.id;
  }
}
