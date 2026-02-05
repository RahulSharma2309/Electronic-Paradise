/**
 * Branding Configuration
 * Central configuration for application branding, theming, and white-label customization
 * 
 * This file contains all brand-specific values that can be easily changed for different deployments
 * or white-label instances. Update these values to rebrand the entire application.
 */

// ============================================================================
// BRAND IDENTITY
// ============================================================================

export const BRAND = {
  // Application name
  APP_NAME: "FreshHarvest Market",
  APP_NAME_SHORT: "FreshHarvest",
  
  // Taglines and marketing copy
  TAGLINE: "Pure, Certified, Delivered Fresh",
  LANDING_TITLE: "Welcome to FreshHarvest",
  LANDING_SUBTITLE: "100% certified organic products from trusted farms to your doorstep",
  
  // SEO and metadata
  META_DESCRIPTION: "Shop fresh organic produce, grains, and groceries. Certified organic products delivered to your door.",
  META_KEYWORDS: "organic, fresh produce, organic vegetables, organic fruits, natural food",
};

// ============================================================================
// THEME COLORS (Organic/Natural Palette)
// ============================================================================

export const THEME = {
  // Primary colors - Fresh Green (represents growth, nature, organic)
  PRIMARY: "#2E7D32",           // Deep Forest Green
  PRIMARY_DARK: "#1B5E20",      // Darker Forest Green
  PRIMARY_LIGHT: "#4CAF50",     // Light Green
  PRIMARY_LIGHTER: "#81C784",   // Lighter Green
  
  // Secondary colors - Earth Tones
  SECONDARY: "#795548",         // Brown (earth, soil)
  SECONDARY_DARK: "#5D4037",    // Dark Brown
  SECONDARY_LIGHT: "#A1887F",   // Light Brown
  
  // Accent colors - Fresh & Vibrant
  ACCENT: "#8BC34A",            // Light Green (fresh produce)
  ACCENT_WARM: "#FFA726",       // Warm Orange (sunshine, citrus)
  ACCENT_COOL: "#26A69A",       // Teal (water, freshness)
  
  // Utility colors
  SUCCESS: "#4CAF50",           // Green
  WARNING: "#FFC107",           // Amber
  ERROR: "#F44336",             // Red
  INFO: "#2196F3",              // Blue
  
  // Background colors
  BG_PRIMARY: "#FAFAFA",        // Light gray
  BG_SECONDARY: "#F5F5F5",      // Slightly darker gray
  BG_CARD: "#FFFFFF",           // White
  
  // Text colors
  TEXT_PRIMARY: "#212121",      // Almost black
  TEXT_SECONDARY: "#757575",    // Gray
  TEXT_LIGHT: "#FFFFFF",        // White
  
  // Gradients
  GRADIENT_PRIMARY: "linear-gradient(135deg, #2E7D32 0%, #4CAF50 100%)",
  GRADIENT_HERO: "linear-gradient(135deg, #1B5E20 0%, #2E7D32 50%, #4CAF50 100%)",
  GRADIENT_ACCENT: "linear-gradient(135deg, #8BC34A 0%, #4CAF50 100%)",
};

// ============================================================================
// ICONS & EMOJIS
// ============================================================================

export const ICONS = {
  // Shopping
  CART: "🛒",
  CHECKOUT: "💳",
  WALLET: "💰",
  
  // Organic & Nature
  ORGANIC: "🌱",
  LEAF: "🍃",
  SPROUT: "🌿",
  CERTIFIED: "✓",
  CERTIFICATE: "📜",
  
  // Products
  FRUITS: "🍎",
  VEGETABLES: "🥬",
  GRAINS: "🌾",
  DAIRY: "🥛",
  FRESH: "✨",
  
  // Status
  STOCK: "📦",
  LOW_STOCK: "⚠️",
  OUT_OF_STOCK: "❌",
  AVAILABLE: "✅",
  
  // User
  USER: "👤",
  PROFILE: "👤",
  LOGIN: "🔐",
  LOGOUT: "🚪",
  
  // Actions
  SEARCH: "🔍",
  FILTER: "🔽",
  HEART: "❤️",
  STAR: "⭐",
};

// ============================================================================
// CURRENCY CONFIGURATION
// ============================================================================

export const CURRENCY = {
  CODE: "INR",
  SYMBOL: "₹",
  LOCALE: "en-IN",
  DECIMAL_PLACES: 2,
  
  // Formatting options
  FORMAT: {
    style: "currency",
    currency: "INR",
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  },
};

// ============================================================================
// UI TEXT & LABELS (for easy localization)
// ============================================================================

export const UI_TEXT = {
  // Navigation
  NAV_HOME: "Home",
  NAV_SHOP: "Shop",
  NAV_CART: "Cart",
  NAV_ORDERS: "Orders",
  NAV_PROFILE: "Profile",
  
  // Product pages
  SHOP_TITLE: "Shop Organic Products",
  SEARCH_PLACEHOLDER: "Search organic products...",
  NO_PRODUCTS: "No organic products available",
  ADD_TO_CART: "Add to Cart",
  OUT_OF_STOCK: "Out of Stock",
  LOW_STOCK_WARNING: "Only {count} left in stock!",
  
  // Cart
  CART_TITLE: "Your Cart",
  CART_EMPTY: "Your cart is empty",
  CART_SUBTOTAL: "Subtotal",
  CART_TOTAL: "Total",
  PROCEED_TO_CHECKOUT: "Proceed to Checkout",
  
  // Checkout
  CHECKOUT_TITLE: "Checkout",
  CONFIRM_ORDER: "Confirm & Pay",
  ORDER_SUCCESS: "Order placed successfully!",
  
  // Orders
  ORDER_HISTORY_TITLE: "Order History",
  NO_ORDERS: "You haven't placed any orders yet",
  ORDER_STATUS: {
    PENDING: "Pending",
    PROCESSING: "Processing",
    SHIPPED: "Shipped",
    DELIVERED: "Delivered",
    CANCELLED: "Cancelled",
  },
  
  // Profile
  PROFILE_TITLE: "My Profile",
  WALLET_BALANCE: "Wallet Balance",
  ADD_BALANCE: "Add Balance",
  
  // Auth
  LOGIN_TITLE: "Login to FreshHarvest",
  SIGNUP_TITLE: "Join FreshHarvest",
  LOGOUT: "Logout",
  
  // Product details
  PRODUCT_DETAILS: "Product Details",
  CATEGORY: "Category",
  BRAND: "Brand",
  ORIGIN: "Origin",
  CERTIFICATION: "Certification",
  UNIT: "Unit",
  STOCK_STATUS: "Stock Status",
};

// ============================================================================
// PRODUCT CATEGORIES (Organic-specific)
// ============================================================================

export const CATEGORIES = {
  FRUITS: {
    id: "fruits",
    name: "Fresh Fruits",
    icon: ICONS.FRUITS,
    description: "Organic fruits picked at peak ripeness",
  },
  VEGETABLES: {
    id: "vegetables",
    name: "Fresh Vegetables",
    icon: ICONS.VEGETABLES,
    description: "Farm-fresh organic vegetables",
  },
  GRAINS: {
    id: "grains",
    name: "Grains & Pulses",
    icon: ICONS.GRAINS,
    description: "Organic grains, rice, and pulses",
  },
  DAIRY: {
    id: "dairy",
    name: "Dairy & Eggs",
    icon: ICONS.DAIRY,
    description: "Organic dairy products and free-range eggs",
  },
  HERBS: {
    id: "herbs",
    name: "Herbs & Spices",
    icon: ICONS.LEAF,
    description: "Fresh organic herbs and natural spices",
  },
};

// ============================================================================
// CERTIFICATION TYPES
// ============================================================================

export const CERTIFICATIONS = {
  INDIA_ORGANIC: {
    id: "india-organic",
    name: "India Organic",
    description: "Certified by Food Safety and Standards Authority of India",
    logo: "🇮🇳",
  },
  USDA_ORGANIC: {
    id: "usda-organic",
    name: "USDA Organic",
    description: "United States Department of Agriculture Organic Certification",
    logo: "🇺🇸",
  },
  EU_ORGANIC: {
    id: "eu-organic",
    name: "EU Organic",
    description: "European Union Organic Certification",
    logo: "🇪🇺",
  },
};

// ============================================================================
// FEATURE FLAGS (for white-label customization)
// ============================================================================

export const FEATURES = {
  SHOW_CERTIFICATIONS: true,
  SHOW_ORIGIN: true,
  SHOW_BRAND: true,
  SHOW_EXPIRATION_DATE: true,
  ENABLE_WISHLIST: false,
  ENABLE_REVIEWS: false,
  ENABLE_RATINGS: false,
};

// ============================================================================
// SOCIAL & CONTACT
// ============================================================================

export const CONTACT = {
  EMAIL: "support@freshharvestmarket.com",
  PHONE: "+91-1800-123-4567",
  ADDRESS: "123 Organic Lane, Green Valley, Karnataka, India",
  
  // Social media (optional)
  SOCIAL: {
    FACEBOOK: "",
    INSTAGRAM: "",
    TWITTER: "",
  },
};

// ============================================================================
// EXPORT DEFAULT CONFIG
// ============================================================================

export default {
  BRAND,
  THEME,
  ICONS,
  CURRENCY,
  UI_TEXT,
  CATEGORIES,
  CERTIFICATIONS,
  FEATURES,
  CONTACT,
};
