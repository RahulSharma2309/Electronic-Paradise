# 📅 Sprint-by-Sprint Guide - Product Evolution

> **See exactly what happens in each sprint, what users will experience, and what you need to focus on as Product Owner**

---

## 🎯 How to Use This Guide

**Before each sprint:**
1. Read the sprint section below
2. Understand what users will experience
3. Review what you need to focus on
4. Prepare for sprint planning

**During the sprint:**
- Reference this guide when reviewing work
- Use the demo script to validate features
- Check the PO focus areas

**After the sprint:**
- Use the demo script in sprint review
- Validate against the success criteria
- Plan for the next sprint

---

## 📊 Current Status

**✅ MVP Complete (Sprint 0)**
- Basic e-commerce functionality working
- Users can: sign up, browse, add to cart, checkout, view orders

**🚧 Next: Epic 1 - Enhanced Product Domain**
- Starting with Sprint 1
- Goal: Make catalog feel like a real electronics store

---

## 🗓️ Sprint Overview (All 35 Sprints)

### Q1: Enhanced Catalog (Sprints 1-6)
**Theme:** Make product browsing and selection feel like electronics commerce

| Sprint | Theme | What Users See |
|--------|-------|----------------|
| 1 | Categories | Products organized by type (Smartphones, Laptops, etc.) |
| 2 | Variants | Can select color/storage options, see different prices |
| 3 | Pricing + Specs | See discounts clearly, view product specifications |
| 4 | Media + Search | Product galleries, search and filter functionality |
| 5 | Inventory + Reviews | Stock alerts, customer reviews and ratings |
| 6 | Wishlist + Compare | Save items, compare products side-by-side |

### Q2: Trustworthy Orders (Sprints 7-11)
**Theme:** Make purchasing and post-purchase trustworthy and explicit

| Sprint | Theme | What Users See |
|--------|-------|----------------|
| 7 | Order Lifecycle | Clear order status with progression |
| 8 | Cancel/Modify | Can cancel orders, get refunds |
| 9 | Reliability + Invoice | Fewer errors, can download invoices |
| 10 | Multi-payments | Choose payment method (card, UPI, wallet) |
| 11 | Coupons + Retry | Apply promo codes, retry failed payments |

### Q3: Frontend & Testing (Sprints 12-18)
**Theme:** Modernize frontend and add comprehensive testing

| Sprint | Theme | What Users See |
|--------|-------|----------------|
| 12 | React Query | Faster page loads, less loading flicker |
| 13 | Global State + Forms | Better forms, fewer UI bugs |
| 14 | PWA + A11y | Can install as app, more accessible |
| 15 | Errors + Micro-UX | Better error messages, smoother interactions |
| 16 | Storybook + Perf | Consistent UI, faster performance |
| 17 | Backend Tests | Fewer bugs, more stable |
| 18 | Integration + E2E | More reliable, tested user journeys |

### Q4: DevOps & Operations (Sprints 19-27)
**Theme:** Automate deployment and add observability

| Sprint | Theme | What Users See |
|--------|-------|----------------|
| 19 | CI Basics | Faster deployments, visible build status |
| 20 | Versioning/Quality | Better release management |
| 21 | K8s Setup | Deployable to cloud |
| 22 | Helm + Ingress | Easier rollbacks, stable routing |
| 23 | Storage + Scaling | Handles more traffic |
| 24 | Mesh/GitOps | Advanced operations (optional) |
| 25 | Logging | Better error tracking |
| 26 | Metrics/Dashboards | System health visible |
| 27 | Tracing | Performance monitoring |

### Q5: Growth & Security (Sprints 28-35)
**Theme:** Add growth features and security hardening

| Sprint | Theme | What Users See |
|--------|-------|----------------|
| 28 | Notifications | Order updates, confirmations |
| 29 | Recommendations | "Customers also bought" suggestions |
| 30-31 | Admin | Admin dashboard for managing store |
| 32 | Advanced Search | Better search, real-time updates |
| 33 | Caching + Rate Limit | Faster responses, protection from abuse |
| 34 | Security Hardening | More secure, protected data |
| 35 | Security Testing | Security audits, compliance |

---

## 📝 Detailed Sprint Breakdowns

### Sprint 0: MVP Baseline ✅ COMPLETE

**What users can do:**
- Register and log in
- Browse products (basic list)
- Add items to cart
- Checkout using wallet payment
- View order history
- Add money to wallet

**What's working:**
- 5 microservices (Auth, User, Product, Order, Payment)
- API Gateway
- Basic frontend
- Docker setup

**Status:** ✅ Complete

---

### Sprint 1: Categories & Type System 🚧 NEXT

**Epic:** Epic 1 - Enhanced Product Domain  
**PBI:** PBI 1.1 - Product Categories & Types  
**Story Points:** 13

#### What Users Will Experience

**Before Sprint 1:**
- Products are just a flat list
- No organization by type
- Hard to find specific categories

**After Sprint 1:**
- Products organized into categories:
  - 📱 Smartphones
  - 💻 Laptops
  - ⌚ Smart Watches
  - 📟 Tablets
  - 🎧 Audio Devices
  - 📷 Cameras
  - 🎮 Gaming Devices
  - 🖥️ Computer Accessories
- Can browse by category
- Can filter by category
- Product detail pages show category

**User Journey:**
1. User visits homepage
2. Sees category navigation (like Amazon's department menu)
3. Clicks "Smartphones"
4. Sees only smartphones
5. Can filter further within category

#### What You Need to Focus On (PO)

**Acceptance Criteria:**
- [ ] Products have categories assigned
- [ ] Categories are displayed in navigation
- [ ] Can filter products by category
- [ ] Product detail pages show category
- [ ] API endpoints support category filtering

**Questions to Ask:**
- How are categories structured? (hierarchy or flat?)
- Can a product be in multiple categories?
- What happens to existing products without categories?

**Demo Script:**
1. Show homepage with category navigation
2. Click "Smartphones" category
3. Show filtered product list
4. Click a product
5. Show product detail with category displayed
6. Show category filter working

**Success Metrics:**
- Users can find products faster
- Category navigation is intuitive
- Filtering works correctly

**Related Documents:**
- [Epic 1](../4-epics-and-pbis/EPIC_1/EPIC_1.md)
- [PBI 1.1](../4-epics-and-pbis/EPIC_1/EPIC_1_PBI_1_1.md)

---

### Sprint 2: Variant Selection

**Epic:** Epic 1 - Enhanced Product Domain  
**PBI:** PBI 1.2 - Product Variants (SKUs)  
**Story Points:** 13

#### What Users Will Experience

**Before Sprint 2:**
- Products are single items
- No color/storage options
- Can't see different configurations

**After Sprint 2:**
- Products have variants:
  - **Example:** iPhone 15 Pro
    - Colors: Space Gray, Silver, Gold, Blue
    - Storage: 128GB, 256GB, 512GB, 1TB
    - Total: 16 variants (4 colors × 4 storage)
- Can select variant on product page
- Price changes based on variant
- Stock shown per variant
- SKU (Stock Keeping Unit) for each variant

**User Journey:**
1. User opens iPhone 15 Pro product page
2. Sees variant selector (Color and Storage dropdowns)
3. Selects "Space Gray" and "256GB"
4. Price updates: $999 → $1,099
5. Stock shows: "In Stock" or "Only 3 left"
6. Adds to cart with selected variant

#### What You Need to Focus On (PO)

**Acceptance Criteria:**
- [ ] Products can have multiple variants
- [ ] Variant selector works on product page
- [ ] Price updates when variant changes
- [ ] Stock shown per variant
- [ ] Cart stores selected variant
- [ ] SKU generated for each variant

**Questions to Ask:**
- How are variants structured? (all combinations or specific ones?)
- What if a variant combination doesn't exist?
- How do we handle variant pricing?
- What about variant images?

**Demo Script:**
1. Open product with variants (e.g., iPhone)
2. Show variant selector
3. Change color → price/stock updates
4. Change storage → price/stock updates
5. Add to cart
6. Show cart with variant selected
7. Show checkout with correct variant

**Success Metrics:**
- Users can select variants easily
- Price/stock updates correctly
- No confusion about which variant is selected

**Related Documents:**
- [Epic 1](../4-epics-and-pbis/EPIC_1/EPIC_1.md)
- [PBI 1.2](../4-epics-and-pbis/EPIC_1/EPIC_1_PBI_1_2.md)

---

### Sprint 3: Pricing + Specs

**Epic:** Epic 1 - Enhanced Product Domain  
**PBI:** PBI 1.3 - Dynamic Pricing, PBI 1.4 - Product Specifications  
**Story Points:** 13 + 13 = 26

#### What Users Will Experience

**Pricing:**
- See original price and sale price
- See discount percentage (e.g., "20% OFF")
- Understand pricing strategies:
  - Regular price
  - Sale price
  - Bundle discounts
  - Seasonal pricing
- Transparent pricing display

**Specifications:**
- Product detail pages show specifications:
  - **Smartphone:** Screen size, RAM, Storage, Camera, Battery
  - **Laptop:** Processor, RAM, Storage, Graphics, Screen size
  - **Watch:** Display type, Battery life, Fitness features
- Specs organized by category
- Can filter products by specs

**User Journey:**
1. User opens product page
2. Sees pricing: "Was $1,199, Now $999 (17% OFF)"
3. Scrolls to specifications section
4. Sees detailed specs organized clearly
5. Can filter other products by similar specs

#### What You Need to Focus On (PO)

**Pricing Acceptance Criteria:**
- [ ] Multiple pricing strategies supported
- [ ] Discounts displayed clearly
- [ ] Price calculation is correct
- [ ] Pricing rules are testable

**Specs Acceptance Criteria:**
- [ ] Products have specifications
- [ ] Specs displayed on product page
- [ ] Can filter by specifications
- [ ] Specs organized by category

**Questions to Ask:**
- What pricing strategies do we support?
- How are specs stored? (flexible or fixed?)
- Can specs vary by product type?
- How do we handle missing specs?

**Demo Script:**
1. Show product with sale price
2. Explain discount calculation
3. Show specifications section
4. Filter products by spec (e.g., "16GB RAM")
5. Show filtered results

**Success Metrics:**
- Users trust pricing
- Users can compare products by specs
- Pricing calculations are accurate

**Related Documents:**
- [PBI 1.3](../4-epics-and-pbis/EPIC_1/EPIC_1_PBI_1_3.md) - Dynamic Pricing
- [PBI 1.4](../4-epics-and-pbis/EPIC_1/EPIC_1_PBI_1_4.md) - Product Specifications

---

### Sprint 4: Media + Search/Filter

**Epic:** Epic 1 - Enhanced Product Domain  
**PBI:** PBI 1.5 - Product Images, PBI 1.6 - Search & Filtering  
**Story Points:** 13 + 21 = 34

#### What Users Will Experience

**Media:**
- Product pages have image galleries
- Multiple images per product
- Can zoom images
- Primary image displayed
- Image carousel/slider

**Search & Filter:**
- Search bar in header
- Can search by product name, brand, description
- Advanced filters:
  - Category
  - Price range
  - Brand
  - Specifications (RAM, storage, etc.)
- Sort options:
  - Price: Low to High
  - Price: High to Low
  - Newest First
  - Best Rated
- Pagination (show 20 products per page)

**User Journey:**
1. User types "laptop" in search
2. Sees search results
3. Applies filters: "Price: $500-$1000", "RAM: 16GB"
4. Sorts by "Price: Low to High"
5. Clicks product
6. Sees image gallery with zoom
7. Can browse through images

#### What You Need to Focus On (PO)

**Media Acceptance Criteria:**
- [ ] Products have multiple images
- [ ] Image gallery works
- [ ] Can zoom images
- [ ] Primary image displayed correctly
- [ ] Images load quickly

**Search Acceptance Criteria:**
- [ ] Search works by name, brand, description
- [ ] Filters work correctly
- [ ] Sort options work
- [ ] Pagination works
- [ ] Search is fast enough

**Questions to Ask:**
- How many images per product?
- Image storage location? (local or cloud?)
- Search performance requirements?
- What filters are most important?
- How many results per page?

**Demo Script:**
1. Search "laptop"
2. Apply multiple filters
3. Sort results
4. Click product
5. Show image gallery
6. Zoom an image
7. Navigate through images

**Success Metrics:**
- Users can find products quickly
- Search results are relevant
- Filters work correctly
- Images load fast

**Related Documents:**
- [PBI 1.5](../4-epics-and-pbis/EPIC_1/) - Product Images (when created)
- [PBI 1.6](../4-epics-and-pbis/EPIC_1/) - Search & Filtering (when created)

---

### Sprint 5: Inventory + Reviews

**Epic:** Epic 1 - Enhanced Product Domain  
**PBI:** PBI 1.7 - Inventory Management, PBI 1.8 - Reviews & Ratings  
**Story Points:** 13 + 13 = 26

#### What Users Will Experience

**Inventory:**
- Stock status displayed clearly:
  - "In Stock"
  - "Only 3 left"
  - "Out of Stock"
  - "Backorder Available"
- Low stock alerts
- Stock updates in real-time

**Reviews & Ratings:**
- Product pages show:
  - Average rating (e.g., 4.5 stars)
  - Number of reviews
  - Review breakdown (5 stars, 4 stars, etc.)
- Can read reviews:
  - Review title
  - Rating
  - Review text
  - Reviewer name (or "Verified Buyer")
  - Date
- Can write reviews (if purchased)
- Can rate products

**User Journey:**
1. User opens product page
2. Sees stock status: "Only 2 left - Order soon!"
3. Scrolls to reviews section
4. Sees average rating: 4.5/5 (127 reviews)
5. Reads a few reviews
6. After purchase, can leave review

#### What You Need to Focus On (PO)

**Inventory Acceptance Criteria:**
- [ ] Stock status displayed
- [ ] Low stock alerts work
- [ ] Stock updates correctly
- [ ] Can't add out-of-stock to cart

**Reviews Acceptance Criteria:**
- [ ] Can view reviews
- [ ] Can write reviews (if purchased)
- [ ] Ratings displayed correctly
- [ ] Review moderation (if needed)
- [ ] Verified buyer badge

**Questions to Ask:**
- What stock thresholds trigger alerts?
- Who can write reviews? (all users or verified buyers only?)
- Review moderation process?
- How are ratings calculated?

**Demo Script:**
1. Show product with low stock alert
2. Show reviews section
3. Read a review
4. Show rating breakdown
5. (If purchased) Show review form
6. Submit review
7. Show review appears

**Success Metrics:**
- Users trust stock information
- Reviews help decision-making
- Review submission works

**Related Documents:**
- [PBI 1.7](../4-epics-and-pbis/EPIC_1/) - Inventory Management (when created)
- [PBI 1.8](../4-epics-and-pbis/EPIC_1/) - Reviews & Ratings (when created)

---

### Sprint 6: Wishlist + Comparison

**Epic:** Epic 1 - Enhanced Product Domain  
**PBI:** PBI 1.9 - Wishlist, PBI 1.10 - Product Comparison  
**Story Points:** 13 + 8 = 21

#### What Users Will Experience

**Wishlist:**
- Can add products to wishlist
- Can view wishlist
- Can remove from wishlist
- Wishlist persists (saved to account)
- Can add wishlist items to cart
- Get notified if wishlist item price drops (future)

**Comparison:**
- Can select products to compare (up to 4)
- Comparison page shows:
  - Side-by-side product details
  - Specifications comparison
  - Price comparison
  - Rating comparison
- Highlights differences
- Can only compare similar products (same category)

**User Journey:**
1. User browses products
2. Clicks "Add to Wishlist" on 3 products
3. Views wishlist page
4. Selects 2 products to compare
5. Sees comparison page
6. Sees differences highlighted
7. Makes decision based on comparison

#### What You Need to Focus On (PO)

**Wishlist Acceptance Criteria:**
- [ ] Can add/remove from wishlist
- [ ] Wishlist persists
- [ ] Can view wishlist
- [ ] Can add wishlist items to cart

**Comparison Acceptance Criteria:**
- [ ] Can select products to compare (max 4)
- [ ] Comparison page shows key details
- [ ] Differences highlighted
- [ ] Can only compare similar products
- [ ] Comparison works correctly

**Questions to Ask:**
- How many items in wishlist? (unlimited?)
- What products can be compared? (same category only?)
- What details shown in comparison?
- How are differences highlighted?

**Demo Script:**
1. Add products to wishlist
2. View wishlist
3. Select products to compare
4. Show comparison page
5. Highlight differences
6. Add one to cart from comparison

**Success Metrics:**
- Users use wishlist
- Comparison helps decisions
- Users return to wishlist

**Related Documents:**
- [PBI 1.9](../4-epics-and-pbis/EPIC_1/) - Wishlist (when created)
- [PBI 1.10](../4-epics-and-pbis/EPIC_1/) - Product Comparison (when created)

---

## 🎯 Epic 1 Complete!

**After Sprint 6, Epic 1 is complete!**

**What we've built:**
- ✅ Professional product catalog
- ✅ Categories and types
- ✅ Product variants
- ✅ Dynamic pricing
- ✅ Product specifications
- ✅ Image galleries
- ✅ Search and filtering
- ✅ Inventory management
- ✅ Reviews and ratings
- ✅ Wishlist
- ✅ Product comparison

**What users can do:**
- Find products easily
- Compare options
- Make informed decisions
- Save items for later
- Read reviews
- Trust the pricing

**Next:** Epic 2 - Advanced Order Management

---

## 📋 Sprint 7-11: Epic 2-3 (Trustworthy Orders)

### Sprint 7: Order Lifecycle

**What users see:**
- Clear order status:
  - Pending
  - Processing
  - Shipped
  - Delivered
  - Cancelled
- Order status updates
- Order history with status
- Status progression visible

**PO Focus:**
- State machine rules
- Status transitions
- History tracking

---

### Sprint 8: Cancel/Modify Orders

**What users see:**
- Can cancel orders (when allowed)
- Automatic refunds
- Clear cancellation rules
- Can modify orders (before shipping)

**PO Focus:**
- Cancellation rules
- Refund logic
- Modification constraints

---

### Sprint 9: Reliability + Invoice

**What users see:**
- Fewer errors
- Better error handling
- Can download invoices
- Invoices emailed automatically

**PO Focus:**
- Error handling
- Invoice format
- Email delivery

---

### Sprint 10: Multi-Payment Methods

**What users see:**
- Choose payment method:
  - Wallet (existing)
  - Credit/Debit Card
  - UPI
  - Net Banking
- Clear payment status
- Payment retry options

**PO Focus:**
- Payment adapter interfaces
- Payment method selection
- Error handling

---

### Sprint 11: Coupons + Payment Retry

**What users see:**
- Can apply coupon codes
- Discount shown clearly
- Payment retry if fails
- Clear error messages

**PO Focus:**
- Coupon validation
- Retry logic
- Error messaging

---

## 📚 How to Use This Guide

### Before Each Sprint

1. **Read the sprint section** - Understand what's coming
2. **Review related documents** - Read Epic and PBI docs
3. **Prepare questions** - List what's unclear
4. **Review acceptance criteria** - Know what "done" means
5. **Prepare demo script** - Know what to validate

### During Sprint

1. **Reference this guide** - Check what users should see
2. **Review work in progress** - Validate against criteria
3. **Test features** - Use the demo script
4. **Answer questions** - Help the team

### After Sprint

1. **Use demo script** - In sprint review
2. **Validate success** - Check metrics
3. **Plan next sprint** - Review next section

---

## 🔗 Related Documents

- [Product Roadmap](../3-product-owner/PRODUCT_ROADMAP_PM_PO.md) - High-level roadmap
- [Project Roadmap](../9-roadmap-and-tracking/PROJECT_ROADMAP.md) - Detailed roadmap
- [Iteration Checklist](../9-roadmap-and-tracking/ITERATION_CHECKLIST.md) - Progress tracking
- [Epic 1](../4-epics-and-pbis/EPIC_1/EPIC_1.md) - Epic details
- [Definition of Done](../3-product-owner/DEFINITION_OF_DONE.md) - What "done" means

---

**Remember:** This is a living document. As we progress, we'll add more detail to future sprints. Focus on the current and next sprint!

**Last Updated:** January 2026  
**Version:** 1.0
