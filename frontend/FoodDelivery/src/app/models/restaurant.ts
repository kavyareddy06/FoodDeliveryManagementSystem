export interface Restaurant {
    restaurantId: number;
    name: string;
    cuisineType: string;
    location: string;
    priceRange: string;
    description: string;
    imageUrl?: string; // Optional, for the image URL
  }