import { Component } from '@angular/core';
import { MenuService } from '../../services/menu.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router'; // Import ActivatedRoute

@Component({
  selector: 'app-add-menu-item',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-menu-item.component.html',
  styleUrls: ['./add-menu-item.component.css']
})
export class AddMenuItemComponent {
  menuItems: any[] = [];
  
  urlrestaurantId:number=0;
  editMenuItemId: number | null = null;

  constructor(
    private menuService: MenuService,
    private toastr: ToastrService,
    private route: ActivatedRoute // Inject ActivatedRoute
    
  ) {}

  ngOnInit(): void {
     
    this.route.params.subscribe(params => {
      this.urlrestaurantId = +params['restaurantId'];  
      console.log('Restaurant ID from URL:', this.urlrestaurantId);
      this.getMenuItems(); 
     // localStorage.setItem('resId',this.restaurantId);
      
     // this.getPromotions(this.restaurantId);
    });
  }
  newMenuItem = {
   itemId: 0,
    restaurantId: this.urlrestaurantId, // We'll set this from the URL
    name: '',
    category: '',
    price: 0,
    description: '',
    availability: false,
  };

  // Extract restaurantId from the URL
  

  resetForm(): void {
    this.newMenuItem = { name: '', itemId: 0, restaurantId:this.urlrestaurantId, category: '', price: 0, description: '', availability: false };
    this.editMenuItemId = null;
  }

  getMenuItems(): void {
    this.menuService.getMenuItems(this.urlrestaurantId).subscribe(
      (data: any) => {
        this.menuItems = data.$values || [];
      },
      error => {
        console.error('Error fetching menu items:', error);
      }
    );
  }

  editMenuItem(itemId: number): void {
    const itemToEdit = this.menuItems.find(item => item.itemId === itemId);

    if (itemToEdit) {
      this.newMenuItem = {
        itemId: itemToEdit.itemId,
        restaurantId: this.urlrestaurantId, // Keep the restaurantId from the URL
        name: itemToEdit.name,
        category: itemToEdit.category,
        price: itemToEdit.price,
        description: itemToEdit.description,
        availability: itemToEdit.availability
      };
      this.editMenuItemId = itemId;
    } else {
      console.error('Item not found for editing');
    }
  }

  addMenuItem(): void {
    const restaurantId = this.urlrestaurantId;
    if (!restaurantId) {
      console.log('No restaurant ID found.');
      return;
    }

    if (this.newMenuItem.name.trim() && this.newMenuItem.category.trim() && this.newMenuItem.price !== null) {
      console.log(this.urlrestaurantId);
      this.newMenuItem.restaurantId = this.urlrestaurantId;
      this.menuService.addMenuItem(this.newMenuItem).subscribe(
        data => {
          if (data && data.message === 'Menu item created successfully.') {
            this.menuItems.push(data.data);
            this.resetForm();
          }
        },
        error => {
          console.error('Error adding menu item:', error);
        }
      );
    } else {
      this.toastr.info('Please fill out all fields');
    }
  }

  deleteMenuItem(id: number): void {
    this.menuService.deleteMenuItem(id).subscribe(
      () => {
        this.menuItems = this.menuItems.filter(item => item.itemId !== id);
      },
      error => {
        console.error('Error deleting menu item:', error);
      }
    );
  }

  saveUpdatedMenuItem(): void {
    if (this.editMenuItemId && this.newMenuItem.name.trim()) {
      const updatedItem = {
        itemId: this.newMenuItem.itemId,
        name: this.newMenuItem.name,
        category: this.newMenuItem.category,
        price: this.newMenuItem.price,
        description: this.newMenuItem.description,
        availability: this.newMenuItem.availability,
      };

      console.log("Updating item:", updatedItem);

      this.menuService.updateMenuItem(this.editMenuItemId, updatedItem).subscribe(
        (response: any) => {
          if (response && response.updatedItem) {
            const index = this.menuItems.findIndex(item => item.itemId === this.editMenuItemId);
            if (index !== -1) {
              this.menuItems[index] = response.updatedItem;
            }
            this.resetForm();
            this.toastr.success('Update successful:', response.message);
          } else {
            this.toastr.warning('Unexpected response format:', response);
          }
        },
        error => {
          console.error('Error updating menu item:', error);
        }
      );
    } else {
      console.warn('Invalid form data. Please ensure all required fields are filled.');
    }
  }
}
