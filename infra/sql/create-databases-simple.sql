-- ============================================================================
-- FreshHarvest Market - Product Service Seed Data
-- Run this script against EP_Local_ProductDb or EP_Staging_ProductDb
-- ============================================================================

USE EP_Local_ProductDb;
GO

-- ============================================================================
-- STEP 1: Clear existing data (optional - comment out if you want to preserve)
-- ============================================================================
DELETE FROM ProductTags;
DELETE FROM ProductImages;
DELETE FROM Products;
DELETE FROM Tags;
DELETE FROM Categories;
GO

-- ============================================================================
-- STEP 2: Create Categories
-- ============================================================================
DECLARE @CatFruits UNIQUEIDENTIFIER = NEWID();
DECLARE @CatVegetables UNIQUEIDENTIFIER = NEWID();
DECLARE @CatDairy UNIQUEIDENTIFIER = NEWID();
DECLARE @CatGrains UNIQUEIDENTIFIER = NEWID();
DECLARE @CatHerbs UNIQUEIDENTIFIER = NEWID();
DECLARE @CatLegumes UNIQUEIDENTIFIER = NEWID();
DECLARE @CatNuts UNIQUEIDENTIFIER = NEWID();
DECLARE @CatBeverages UNIQUEIDENTIFIER = NEWID();

INSERT INTO Categories (Id, Name, Slug, Description, ParentId, IsActive, CreatedAt) VALUES
(@CatFruits, 'Organic Fruits', 'organic-fruits', 'Fresh organic fruits sourced from certified farms across India', NULL, 1, GETUTCDATE()),
(@CatVegetables, 'Organic Vegetables', 'organic-vegetables', 'Farm-fresh organic vegetables, harvested daily', NULL, 1, GETUTCDATE()),
(@CatDairy, 'Organic Dairy', 'organic-dairy', 'Pure organic dairy products from grass-fed cattle', NULL, 1, GETUTCDATE()),
(@CatGrains, 'Organic Grains & Cereals', 'organic-grains-cereals', 'Whole grains and cereals grown without pesticides', NULL, 1, GETUTCDATE()),
(@CatHerbs, 'Organic Herbs & Spices', 'organic-herbs-spices', 'Aromatic herbs and spices from organic cultivation', NULL, 1, GETUTCDATE()),
(@CatLegumes, 'Organic Legumes & Pulses', 'organic-legumes-pulses', 'Protein-rich organic legumes and pulses', NULL, 1, GETUTCDATE()),
(@CatNuts, 'Organic Nuts & Seeds', 'organic-nuts-seeds', 'Premium quality organic nuts and seeds', NULL, 1, GETUTCDATE()),
(@CatBeverages, 'Organic Beverages', 'organic-beverages', 'Healthy organic drinks and beverages', NULL, 1, GETUTCDATE());

-- ============================================================================
-- STEP 3: Create Tags
-- ============================================================================
DECLARE @TagOrganic UNIQUEIDENTIFIER = NEWID();
DECLARE @TagLocal UNIQUEIDENTIFIER = NEWID();
DECLARE @TagSeasonal UNIQUEIDENTIFIER = NEWID();
DECLARE @TagFarmFresh UNIQUEIDENTIFIER = NEWID();
DECLARE @TagPesticideFree UNIQUEIDENTIFIER = NEWID();
DECLARE @TagNonGMO UNIQUEIDENTIFIER = NEWID();
DECLARE @TagVegan UNIQUEIDENTIFIER = NEWID();
DECLARE @TagGlutenFree UNIQUEIDENTIFIER = NEWID();
DECLARE @TagHighProtein UNIQUEIDENTIFIER = NEWID();
DECLARE @TagSuperFood UNIQUEIDENTIFIER = NEWID();
DECLARE @TagBestSeller UNIQUEIDENTIFIER = NEWID();
DECLARE @TagNewArrival UNIQUEIDENTIFIER = NEWID();

INSERT INTO Tags (Id, Name, Slug) VALUES
(@TagOrganic, 'Organic', 'organic'),
(@TagLocal, 'Local', 'local'),
(@TagSeasonal, 'Seasonal', 'seasonal'),
(@TagFarmFresh, 'Farm Fresh', 'farm-fresh'),
(@TagPesticideFree, 'Pesticide Free', 'pesticide-free'),
(@TagNonGMO, 'Non-GMO', 'non-gmo'),
(@TagVegan, 'Vegan', 'vegan'),
(@TagGlutenFree, 'Gluten Free', 'gluten-free'),
(@TagHighProtein, 'High Protein', 'high-protein'),
(@TagSuperFood, 'Superfood', 'superfood'),
(@TagBestSeller, 'Best Seller', 'best-seller'),
(@TagNewArrival, 'New Arrival', 'new-arrival');

-- ============================================================================
-- STEP 4: Create Products
-- ============================================================================

-- Product ID Variables for linking images and tags later
DECLARE @P1 UNIQUEIDENTIFIER = NEWID();  -- Bananas
DECLARE @P2 UNIQUEIDENTIFIER = NEWID();  -- Apples
DECLARE @P3 UNIQUEIDENTIFIER = NEWID();  -- Mangoes
DECLARE @P4 UNIQUEIDENTIFIER = NEWID();  -- Oranges
DECLARE @P5 UNIQUEIDENTIFIER = NEWID();  -- Strawberries
DECLARE @P6 UNIQUEIDENTIFIER = NEWID();  -- Blueberries
DECLARE @P7 UNIQUEIDENTIFIER = NEWID();  -- Avocados
DECLARE @P8 UNIQUEIDENTIFIER = NEWID();  -- Grapes
DECLARE @P9 UNIQUEIDENTIFIER = NEWID();  -- Pomegranates
DECLARE @P10 UNIQUEIDENTIFIER = NEWID(); -- Raspberries

DECLARE @P11 UNIQUEIDENTIFIER = NEWID(); -- Tomatoes
DECLARE @P12 UNIQUEIDENTIFIER = NEWID(); -- Spinach
DECLARE @P13 UNIQUEIDENTIFIER = NEWID(); -- Carrots
DECLARE @P14 UNIQUEIDENTIFIER = NEWID(); -- Broccoli
DECLARE @P15 UNIQUEIDENTIFIER = NEWID(); -- Kale
DECLARE @P16 UNIQUEIDENTIFIER = NEWID(); -- Bell Peppers
DECLARE @P17 UNIQUEIDENTIFIER = NEWID(); -- Sweet Potato
DECLARE @P18 UNIQUEIDENTIFIER = NEWID(); -- Cauliflower
DECLARE @P19 UNIQUEIDENTIFIER = NEWID(); -- Beets
DECLARE @P20 UNIQUEIDENTIFIER = NEWID(); -- Brussels Sprouts

DECLARE @P21 UNIQUEIDENTIFIER = NEWID(); -- Milk
DECLARE @P22 UNIQUEIDENTIFIER = NEWID(); -- Greek Yogurt
DECLARE @P23 UNIQUEIDENTIFIER = NEWID(); -- Cottage Cheese
DECLARE @P24 UNIQUEIDENTIFIER = NEWID(); -- Cheddar Cheese
DECLARE @P25 UNIQUEIDENTIFIER = NEWID(); -- Kefir

DECLARE @P26 UNIQUEIDENTIFIER = NEWID(); -- Brown Rice
DECLARE @P27 UNIQUEIDENTIFIER = NEWID(); -- Quinoa
DECLARE @P28 UNIQUEIDENTIFIER = NEWID(); -- Oats
DECLARE @P29 UNIQUEIDENTIFIER = NEWID(); -- Millet
DECLARE @P30 UNIQUEIDENTIFIER = NEWID(); -- Buckwheat

DECLARE @P31 UNIQUEIDENTIFIER = NEWID(); -- Turmeric
DECLARE @P32 UNIQUEIDENTIFIER = NEWID(); -- Ginger
DECLARE @P33 UNIQUEIDENTIFIER = NEWID(); -- Basil
DECLARE @P34 UNIQUEIDENTIFIER = NEWID(); -- Cinnamon
DECLARE @P35 UNIQUEIDENTIFIER = NEWID(); -- Oregano

DECLARE @P36 UNIQUEIDENTIFIER = NEWID(); -- Chickpeas
DECLARE @P37 UNIQUEIDENTIFIER = NEWID(); -- Lentils
DECLARE @P38 UNIQUEIDENTIFIER = NEWID(); -- Black Beans
DECLARE @P39 UNIQUEIDENTIFIER = NEWID(); -- Kidney Beans
DECLARE @P40 UNIQUEIDENTIFIER = NEWID(); -- Green Peas

DECLARE @P41 UNIQUEIDENTIFIER = NEWID(); -- Almonds
DECLARE @P42 UNIQUEIDENTIFIER = NEWID(); -- Walnuts
DECLARE @P43 UNIQUEIDENTIFIER = NEWID(); -- Chia Seeds
DECLARE @P44 UNIQUEIDENTIFIER = NEWID(); -- Flaxseeds
DECLARE @P45 UNIQUEIDENTIFIER = NEWID(); -- Pumpkin Seeds

DECLARE @P46 UNIQUEIDENTIFIER = NEWID(); -- Green Tea
DECLARE @P47 UNIQUEIDENTIFIER = NEWID(); -- Chamomile Tea
DECLARE @P48 UNIQUEIDENTIFIER = NEWID(); -- Coconut Water
DECLARE @P49 UNIQUEIDENTIFIER = NEWID(); -- Kombucha
DECLARE @P50 UNIQUEIDENTIFIER = NEWID(); -- Vegetable Juice

-- =========================
-- ORGANIC FRUITS (10 items)
-- =========================
INSERT INTO Products (Id, Name, Slug, Description, Price, Stock, Unit, Sku, CategoryId, Brand, IsOrganic, Origin, FarmName, HarvestDate, BestBefore, CertificationNumber, CertificationType, CertifyingAgency, NutritionJson, SeoTitle, SeoDescription, IsActive, IsFeatured, CreatedAt) VALUES
(@P1, 'Organic Bananas', 'organic-bananas-kerala', 'Premium organic bananas from Kerala, naturally ripened without chemicals. Rich in potassium and perfect for smoothies or healthy snacking.', 4999, 150, 'dozen', 'FRU-BAN-001', @CatFruits, 'Kerala Naturals', 1, 'Kerala, India', 'Wayanad Organic Farms', DATEADD(day, -2, GETUTCDATE()), DATEADD(day, 5, GETUTCDATE()), 'IN-ORG-2024-KL-1001', 'India Organic', 'APEDA', '{"calories":89,"protein":1.1,"carbs":23,"fiber":2.6,"sugar":12,"potassium":358}', 'Buy Organic Bananas Online | Fresh from Kerala', 'Fresh organic bananas from certified Kerala farms. Naturally ripened, pesticide-free. Order now for doorstep delivery.', 1, 1, GETUTCDATE()),

(@P2, 'Organic Apples - Himalayan', 'organic-apples-himalayan', 'Crisp and juicy organic apples from Himalayan orchards. Grown at high altitudes for natural sweetness and rich flavor.', 19999, 80, 'kg', 'FRU-APL-001', @CatFruits, 'Himalayan Harvest', 1, 'Shimla, Himachal Pradesh', 'Kinnaur Apple Orchards', DATEADD(day, -5, GETUTCDATE()), DATEADD(day, 21, GETUTCDATE()), 'IN-ORG-2024-HP-2001', 'India Organic', 'IMO Control', '{"calories":52,"protein":0.3,"carbs":14,"fiber":2.4,"sugar":10,"vitaminC":4.6}', 'Himalayan Organic Apples | Premium Quality', 'Premium organic apples from Himalayan orchards. High-altitude grown for superior taste.', 1, 1, GETUTCDATE()),

(@P3, 'Organic Alphonso Mangoes', 'organic-alphonso-mangoes-ratnagiri', 'The king of mangoes! Authentic Ratnagiri Alphonso mangoes with GI tag. Handpicked at perfect ripeness.', 89999, 25, 'dozen', 'FRU-MNG-001', @CatFruits, 'Ratnagiri Gold', 1, 'Ratnagiri, Maharashtra', 'Devgad Mango Estate', DATEADD(day, -1, GETUTCDATE()), DATEADD(day, 7, GETUTCDATE()), 'IN-ORG-2024-MH-3001', 'India Organic', 'Ecocert', '{"calories":60,"protein":0.8,"carbs":15,"fiber":1.6,"sugar":14,"vitaminA":1082}', 'Buy Authentic Alphonso Mangoes | GI Tagged', 'Genuine Ratnagiri Alphonso mangoes with GI certification. Limited seasonal availability.', 1, 1, GETUTCDATE()),

(@P4, 'Organic Nagpur Oranges', 'organic-nagpur-oranges', 'Famous Nagpur oranges known for their sweetness and juiciness. Rich in Vitamin C and perfect for fresh juice.', 12999, 100, 'kg', 'FRU-ORG-001', @CatFruits, 'Vidarbha Organics', 1, 'Nagpur, Maharashtra', 'Citrus Valley Farm', DATEADD(day, -3, GETUTCDATE()), DATEADD(day, 14, GETUTCDATE()), 'IN-ORG-2024-MH-4001', 'India Organic', 'Control Union', '{"calories":47,"protein":0.9,"carbs":12,"fiber":2.4,"sugar":9,"vitaminC":53}', 'Fresh Nagpur Oranges | Organic Certified', 'Sweet and juicy Nagpur oranges from organic farms. Perfect for fresh juice.', 1, 0, GETUTCDATE()),

(@P5, 'Organic Strawberries', 'organic-strawberries-mahabaleshwar', 'Fresh organic strawberries from Mahabaleshwar hills. Sweet, juicy, and perfect for desserts.', 29999, 40, '500g', 'FRU-STR-001', @CatFruits, 'Hill Fresh', 1, 'Mahabaleshwar, Maharashtra', 'Panchgani Berry Farm', DATEADD(day, -1, GETUTCDATE()), DATEADD(day, 4, GETUTCDATE()), 'IN-ORG-2024-MH-5001', 'India Organic', 'APEDA', '{"calories":32,"protein":0.7,"carbs":8,"fiber":2,"sugar":5,"vitaminC":59}', 'Fresh Organic Strawberries | Mahabaleshwar', 'Premium strawberries from Mahabaleshwar. Hand-picked for freshness.', 1, 1, GETUTCDATE()),

(@P6, 'Organic Blueberries', 'organic-blueberries-nilgiris', 'Antioxidant-rich organic blueberries from Nilgiri hills. Perfect for breakfast bowls and smoothies.', 49999, 30, '250g', 'FRU-BLU-001', @CatFruits, 'Nilgiri Naturals', 1, 'Ooty, Tamil Nadu', 'Blue Hills Estate', DATEADD(day, -2, GETUTCDATE()), DATEADD(day, 7, GETUTCDATE()), 'IN-ORG-2024-TN-6001', 'India Organic', 'Ecocert', '{"calories":57,"protein":0.7,"carbs":14,"fiber":2.4,"sugar":10,"vitaminK":19}', 'Organic Blueberries | Superfood Berry', 'Premium organic blueberries from Nilgiri hills. Rich in antioxidants.', 1, 0, GETUTCDATE()),

(@P7, 'Organic Avocados', 'organic-avocados-coorg', 'Creamy organic avocados from Coorg. Perfect for guacamole, salads, or healthy toast.', 34999, 45, '3 pieces', 'FRU-AVO-001', @CatFruits, 'Coorg Organics', 1, 'Coorg, Karnataka', 'Western Ghats Avocado Farm', DATEADD(day, -4, GETUTCDATE()), DATEADD(day, 6, GETUTCDATE()), 'IN-ORG-2024-KA-7001', 'India Organic', 'IMO Control', '{"calories":160,"protein":2,"carbs":9,"fiber":7,"fat":15,"potassium":485}', 'Fresh Organic Avocados | Coorg Premium', 'Creamy avocados from organic Coorg farms. Rich in healthy fats.', 1, 0, GETUTCDATE()),

(@P8, 'Organic Red Grapes', 'organic-red-grapes-nashik', 'Seedless red grapes from Nashik vineyards. Sweet and perfect for snacking or wine making.', 16999, 60, 'kg', 'FRU-GRP-001', @CatFruits, 'Nashik Valley', 1, 'Nashik, Maharashtra', 'Sahyadri Vineyards', DATEADD(day, -2, GETUTCDATE()), DATEADD(day, 10, GETUTCDATE()), 'IN-ORG-2024-MH-8001', 'India Organic', 'Control Union', '{"calories":69,"protein":0.7,"carbs":18,"fiber":0.9,"sugar":16,"vitaminC":3}', 'Organic Nashik Grapes | Seedless Red', 'Premium seedless red grapes from Nashik vineyards.', 1, 0, GETUTCDATE()),

(@P9, 'Organic Pomegranates', 'organic-pomegranates-solapur', 'Ruby-red organic pomegranates from Solapur. Known for their sweet-tart flavor and health benefits.', 24999, 55, 'kg', 'FRU-POM-001', @CatFruits, 'Solapur Select', 1, 'Solapur, Maharashtra', 'Anar Paradise Farm', DATEADD(day, -3, GETUTCDATE()), DATEADD(day, 14, GETUTCDATE()), 'IN-ORG-2024-MH-9001', 'India Organic', 'APEDA', '{"calories":83,"protein":1.7,"carbs":19,"fiber":4,"sugar":14,"vitaminC":10}', 'Organic Pomegranates | Heart Healthy', 'Fresh organic pomegranates rich in antioxidants. From Solapur farms.', 1, 1, GETUTCDATE()),

(@P10, 'Organic Raspberries', 'organic-raspberries-kodaikanal', 'Delicate organic raspberries from Kodaikanal. Perfect for desserts and morning smoothies.', 59999, 15, '125g', 'FRU-RSP-001', @CatFruits, 'Kodai Berries', 1, 'Kodaikanal, Tamil Nadu', 'Princess of Hills Berry Farm', DATEADD(day, -1, GETUTCDATE()), DATEADD(day, 3, GETUTCDATE()), 'IN-ORG-2024-TN-1001', 'India Organic', 'Ecocert', '{"calories":52,"protein":1.2,"carbs":12,"fiber":6.5,"sugar":4,"vitaminC":26}', 'Fresh Organic Raspberries | Kodaikanal', 'Premium organic raspberries. Limited availability.', 1, 0, GETUTCDATE());

-- =============================
-- ORGANIC VEGETABLES (10 items)
-- =============================
INSERT INTO Products (Id, Name, Slug, Description, Price, Stock, Unit, Sku, CategoryId, Brand, IsOrganic, Origin, FarmName, HarvestDate, BestBefore, CertificationNumber, CertificationType, CertifyingAgency, NutritionJson, SeoTitle, SeoDescription, IsActive, IsFeatured, CreatedAt) VALUES
(@P11, 'Organic Tomatoes', 'organic-tomatoes-pune', 'Vine-ripened organic tomatoes from Pune farms. Perfect red color and rich taste for your curries and salads.', 4599, 200, 'kg', 'VEG-TOM-001', @CatVegetables, 'Pune Fresh', 1, 'Pune, Maharashtra', 'Sahyadri Organic Farms', DATEADD(day, -1, GETUTCDATE()), DATEADD(day, 7, GETUTCDATE()), 'IN-ORG-2024-MH-1101', 'India Organic', 'APEDA', '{"calories":18,"protein":0.9,"carbs":3.9,"fiber":1.2,"sugar":2.6,"lycopene":2573}', 'Fresh Organic Tomatoes | Farm Direct', 'Vine-ripened organic tomatoes from Pune. Perfect for cooking.', 1, 1, GETUTCDATE()),

(@P12, 'Organic Baby Spinach', 'organic-baby-spinach', 'Tender organic baby spinach leaves. Packed with iron and perfect for salads, smoothies, or sautéing.', 7999, 80, '200g', 'VEG-SPN-001', @CatVegetables, 'Green Leaf', 1, 'Bangalore, Karnataka', 'Urban Organic Farm', DATEADD(day, 0, GETUTCDATE()), DATEADD(day, 5, GETUTCDATE()), 'IN-ORG-2024-KA-1201', 'India Organic', 'IMO Control', '{"calories":23,"protein":2.9,"carbs":3.6,"fiber":2.2,"iron":2.7,"vitaminK":483}', 'Organic Baby Spinach | Iron Rich', 'Fresh organic baby spinach. Harvested daily for maximum freshness.', 1, 1, GETUTCDATE()),

(@P13, 'Organic Carrots', 'organic-carrots-ooty', 'Crunchy organic carrots from Ooty highlands. Naturally sweet and rich in beta-carotene.', 3999, 150, 'kg', 'VEG-CAR-001', @CatVegetables, 'Nilgiri Fresh', 1, 'Ooty, Tamil Nadu', 'Mountain View Organic', DATEADD(day, -1, GETUTCDATE()), DATEADD(day, 14, GETUTCDATE()), 'IN-ORG-2024-TN-1301', 'India Organic', 'Ecocert', '{"calories":41,"protein":0.9,"carbs":10,"fiber":2.8,"sugar":5,"vitaminA":835}', 'Organic Carrots | Ooty Fresh', 'Sweet organic carrots from Nilgiri highlands.', 1, 0, GETUTCDATE()),

(@P14, 'Organic Broccoli', 'organic-broccoli-lonavala', 'Fresh organic broccoli florets. Rich in vitamins and perfect for stir-fries and salads.', 8999, 60, '500g', 'VEG-BRC-001', @CatVegetables, 'Western Ghats Produce', 1, 'Lonavala, Maharashtra', 'Hill Valley Organics', DATEADD(day, 0, GETUTCDATE()), DATEADD(day, 6, GETUTCDATE()), 'IN-ORG-2024-MH-1401', 'India Organic', 'Control Union', '{"calories":34,"protein":2.8,"carbs":7,"fiber":2.6,"vitaminC":89,"vitaminK":102}', 'Fresh Organic Broccoli | Superfood', 'Premium organic broccoli. Harvested fresh daily.', 1, 0, GETUTCDATE()),

(@P15, 'Organic Kale', 'organic-kale-dehradun', 'Nutrient-dense organic kale from Dehradun. The ultimate superfood for smoothies and salads.', 9999, 40, '200g', 'VEG-KAL-001', @CatVegetables, 'Uttarakhand Organics', 1, 'Dehradun, Uttarakhand', 'Himalayan Greens', DATEADD(day, 0, GETUTCDATE()), DATEADD(day, 5, GETUTCDATE()), 'IN-ORG-2024-UK-1501', 'India Organic', 'APEDA', '{"calories":49,"protein":4.3,"carbs":9,"fiber":3.6,"vitaminK":704,"vitaminC":120}', 'Organic Kale | Superfood Greens', 'Premium organic kale. Nutrient powerhouse.', 1, 1, GETUTCDATE()),

(@P16, 'Organic Bell Peppers - Mixed', 'organic-bell-peppers-mixed', 'Colorful mix of organic bell peppers - red, yellow, and green. Perfect for salads and stir-fries.', 12999, 70, '500g', 'VEG-BEL-001', @CatVegetables, 'Color Fresh', 1, 'Hosur, Tamil Nadu', 'Rainbow Organic Farm', DATEADD(day, -1, GETUTCDATE()), DATEADD(day, 8, GETUTCDATE()), 'IN-ORG-2024-TN-1601', 'India Organic', 'IMO Control', '{"calories":31,"protein":1,"carbs":6,"fiber":2.1,"vitaminC":128,"vitaminA":157}', 'Organic Bell Peppers | Colorful Mix', 'Fresh mixed bell peppers from organic farms.', 1, 0, GETUTCDATE()),

(@P17, 'Organic Sweet Potatoes', 'organic-sweet-potatoes-odisha', 'Naturally sweet organic sweet potatoes from Odisha. Rich in fiber and perfect for healthy meals.', 5499, 120, 'kg', 'VEG-SWT-001', @CatVegetables, 'Odisha Naturals', 1, 'Bhubaneswar, Odisha', 'Coastal Organic Farms', DATEADD(day, -3, GETUTCDATE()), DATEADD(day, 21, GETUTCDATE()), 'IN-ORG-2024-OD-1701', 'India Organic', 'APEDA', '{"calories":86,"protein":1.6,"carbs":20,"fiber":3,"sugar":4,"vitaminA":709}', 'Organic Sweet Potatoes | Odisha', 'Naturally sweet organic sweet potatoes.', 1, 0, GETUTCDATE()),

(@P18, 'Organic Cauliflower', 'organic-cauliflower-panchgani', 'Fresh organic cauliflower heads from Panchgani. Tender and perfect for gobhi dishes.', 4999, 90, 'piece', 'VEG-CAU-001', @CatVegetables, 'Panchgani Produce', 1, 'Panchgani, Maharashtra', 'Table Top Farm', DATEADD(day, 0, GETUTCDATE()), DATEADD(day, 7, GETUTCDATE()), 'IN-ORG-2024-MH-1801', 'India Organic', 'Ecocert', '{"calories":25,"protein":1.9,"carbs":5,"fiber":2,"vitaminC":48,"vitaminK":16}', 'Fresh Organic Cauliflower | Panchgani', 'Premium organic cauliflower. Farm fresh.', 1, 0, GETUTCDATE()),

(@P19, 'Organic Beetroot', 'organic-beetroot-nashik', 'Deep red organic beetroot from Nashik. Rich in nitrates and perfect for juices and salads.', 5999, 100, 'kg', 'VEG-BET-001', @CatVegetables, 'Nashik Roots', 1, 'Nashik, Maharashtra', 'Red Earth Organics', DATEADD(day, -2, GETUTCDATE()), DATEADD(day, 14, GETUTCDATE()), 'IN-ORG-2024-MH-1901', 'India Organic', 'Control Union', '{"calories":43,"protein":1.6,"carbs":10,"fiber":2.8,"iron":0.8,"folate":109}', 'Organic Beetroot | Nashik Fresh', 'Deep red organic beetroot. Perfect for juicing.', 1, 0, GETUTCDATE()),

(@P20, 'Organic Brussels Sprouts', 'organic-brussels-sprouts', 'Mini cabbage-like organic brussels sprouts. Perfect for roasting with a drizzle of olive oil.', 14999, 35, '500g', 'VEG-BRS-001', @CatVegetables, 'Premium Greens', 1, 'Kodaikanal, Tamil Nadu', 'Cool Climate Organics', DATEADD(day, -1, GETUTCDATE()), DATEADD(day, 8, GETUTCDATE()), 'IN-ORG-2024-TN-2001', 'India Organic', 'Ecocert', '{"calories":43,"protein":3.4,"carbs":9,"fiber":3.8,"vitaminK":177,"vitaminC":85}', 'Organic Brussels Sprouts | Premium', 'Fresh organic brussels sprouts. Perfect for roasting.', 1, 0, GETUTCDATE());

-- ==========================
-- ORGANIC DAIRY (5 items)
-- ==========================
INSERT INTO Products (Id, Name, Slug, Description, Price, Stock, Unit, Sku, CategoryId, Brand, IsOrganic, Origin, FarmName, HarvestDate, BestBefore, CertificationNumber, CertificationType, CertifyingAgency, NutritionJson, SeoTitle, SeoDescription, IsActive, IsFeatured, CreatedAt) VALUES
(@P21, 'Organic A2 Milk', 'organic-a2-milk-gir-cow', 'Pure A2 milk from grass-fed Gir cows. Rich in nutrients and easy to digest. No hormones or antibiotics.', 8999, 50, 'liter', 'DAI-MLK-001', @CatDairy, 'Gir Gau Organic', 1, 'Ahmedabad, Gujarat', 'Panchgavya Dairy Farm', GETUTCDATE(), DATEADD(day, 4, GETUTCDATE()), 'IN-ORG-2024-GJ-2101', 'India Organic', 'APEDA', '{"calories":67,"protein":3.4,"carbs":4.7,"fat":3.9,"calcium":120,"vitaminD":1}', 'Organic A2 Gir Cow Milk | Pure', 'Pure A2 milk from grass-fed Gir cows. Farm fresh daily.', 1, 1, GETUTCDATE()),

(@P22, 'Organic Greek Yogurt', 'organic-greek-yogurt', 'Thick and creamy organic Greek yogurt. High in protein and probiotics. No artificial flavors.', 17999, 40, '400g', 'DAI-YOG-001', @CatDairy, 'Culture Kitchen', 1, 'Pune, Maharashtra', 'Yogurt Culture Farm', DATEADD(day, -1, GETUTCDATE()), DATEADD(day, 14, GETUTCDATE()), 'IN-ORG-2024-MH-2201', 'India Organic', 'Ecocert', '{"calories":100,"protein":10,"carbs":6,"fat":5,"calcium":150,"probiotics":true}', 'Organic Greek Yogurt | High Protein', 'Thick creamy Greek yogurt with live cultures.', 1, 1, GETUTCDATE()),

(@P23, 'Organic Cottage Cheese (Paneer)', 'organic-paneer', 'Fresh organic cottage cheese made from A2 milk. Soft, crumbly, and perfect for Indian dishes.', 24999, 60, '400g', 'DAI-PNR-001', @CatDairy, 'Desi Dairy', 1, 'Jaipur, Rajasthan', 'Royal Organic Dairy', GETUTCDATE(), DATEADD(day, 7, GETUTCDATE()), 'IN-ORG-2024-RJ-2301', 'India Organic', 'IMO Control', '{"calories":265,"protein":18,"carbs":3.6,"fat":20,"calcium":208,"phosphorus":138}', 'Organic Fresh Paneer | A2 Milk', 'Fresh cottage cheese from organic A2 milk.', 1, 0, GETUTCDATE()),

(@P24, 'Organic Aged Cheddar Cheese', 'organic-aged-cheddar', 'Sharp and tangy organic aged cheddar. Aged for 12 months for complex flavor.', 44999, 25, '200g', 'DAI-CHD-001', @CatDairy, 'Artisan Cheese Co', 1, 'Kodaikanal, Tamil Nadu', 'Hill Station Dairy', DATEADD(day, -30, GETUTCDATE()), DATEADD(day, 90, GETUTCDATE()), 'IN-ORG-2024-TN-2401', 'India Organic', 'Ecocert', '{"calories":403,"protein":25,"carbs":1.3,"fat":33,"calcium":721,"sodium":621}', 'Organic Aged Cheddar | 12 Month', 'Premium aged cheddar cheese. Rich and sharp.', 1, 0, GETUTCDATE()),

(@P25, 'Organic Kefir', 'organic-kefir-probiotic', 'Fermented organic kefir drink. Packed with probiotics for gut health. Tangy and refreshing.', 13999, 30, '500ml', 'DAI-KEF-001', @CatDairy, 'Gut Health Co', 1, 'Bangalore, Karnataka', 'Probiotic Paradise', GETUTCDATE(), DATEADD(day, 10, GETUTCDATE()), 'IN-ORG-2024-KA-2501', 'India Organic', 'Control Union', '{"calories":63,"protein":3.5,"carbs":4.7,"fat":3.5,"calcium":125,"probiotics":true}', 'Organic Kefir | Probiotic Drink', 'Probiotic-rich organic kefir for gut health.', 1, 0, GETUTCDATE());

-- ================================
-- ORGANIC GRAINS & CEREALS (5 items)
-- ================================
INSERT INTO Products (Id, Name, Slug, Description, Price, Stock, Unit, Sku, CategoryId, Brand, IsOrganic, Origin, FarmName, HarvestDate, BestBefore, CertificationNumber, CertificationType, CertifyingAgency, NutritionJson, SeoTitle, SeoDescription, IsActive, IsFeatured, CreatedAt) VALUES
(@P26, 'Organic Brown Rice', 'organic-brown-rice-basmati', 'Premium organic brown basmati rice. Long grain, aromatic, and packed with nutrients.', 17999, 100, 'kg', 'GRN-RIC-001', @CatGrains, 'Himalayan Grains', 1, 'Dehradun, Uttarakhand', 'Valley Rice Mills', DATEADD(day, -60, GETUTCDATE()), DATEADD(day, 180, GETUTCDATE()), 'IN-ORG-2024-UK-2601', 'India Organic', 'APEDA', '{"calories":370,"protein":7.9,"carbs":77,"fiber":3.5,"iron":1.5,"magnesium":143}', 'Organic Brown Basmati Rice | Himalayan', 'Premium organic brown basmati from Dehradun.', 1, 1, GETUTCDATE()),

(@P27, 'Organic Quinoa', 'organic-quinoa-white', 'Complete protein organic quinoa. Gluten-free superfood perfect for healthy bowls.', 34999, 60, 'kg', 'GRN-QUN-001', @CatGrains, 'Superfood Store', 1, 'Ladakh, India', 'High Altitude Organics', DATEADD(day, -30, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-LA-2701', 'India Organic', 'Ecocert', '{"calories":368,"protein":14,"carbs":64,"fiber":7,"iron":4.6,"magnesium":197}', 'Organic Quinoa | Complete Protein', 'Gluten-free organic quinoa superfood.', 1, 1, GETUTCDATE()),

(@P28, 'Organic Rolled Oats', 'organic-rolled-oats', 'Heart-healthy organic rolled oats. Perfect for breakfast porridge or overnight oats.', 14999, 80, 'kg', 'GRN-OAT-001', @CatGrains, 'Morning Harvest', 1, 'Punjab, India', 'Golden Fields Organic', DATEADD(day, -45, GETUTCDATE()), DATEADD(day, 270, GETUTCDATE()), 'IN-ORG-2024-PB-2801', 'India Organic', 'IMO Control', '{"calories":389,"protein":13,"carbs":66,"fiber":11,"iron":4.7,"zinc":4}', 'Organic Rolled Oats | Heart Healthy', 'Premium organic oats for healthy breakfast.', 1, 0, GETUTCDATE()),

(@P29, 'Organic Ragi (Finger Millet)', 'organic-ragi-flour', 'Traditional organic ragi flour. Excellent source of calcium. Perfect for dosa and roti.', 9999, 120, 'kg', 'GRN-RAG-001', @CatGrains, 'Traditional Grains', 1, 'Hassan, Karnataka', 'Millet Magic Farms', DATEADD(day, -20, GETUTCDATE()), DATEADD(day, 180, GETUTCDATE()), 'IN-ORG-2024-KA-2901', 'India Organic', 'APEDA', '{"calories":336,"protein":7.3,"carbs":72,"fiber":3.6,"calcium":344,"iron":3.9}', 'Organic Ragi | Traditional Millet', 'Calcium-rich organic ragi flour.', 1, 0, GETUTCDATE()),

(@P30, 'Organic Buckwheat', 'organic-buckwheat-kuttu', 'Gluten-free organic buckwheat (kuttu). Traditional fasting food with complete protein.', 19999, 50, 'kg', 'GRN-BUK-001', @CatGrains, 'Ancient Grains', 1, 'Himachal Pradesh, India', 'Mountain Harvest', DATEADD(day, -40, GETUTCDATE()), DATEADD(day, 300, GETUTCDATE()), 'IN-ORG-2024-HP-3001', 'India Organic', 'Control Union', '{"calories":343,"protein":13,"carbs":71,"fiber":10,"magnesium":231,"manganese":1.3}', 'Organic Buckwheat | Gluten Free', 'Traditional organic kuttu atta.', 1, 0, GETUTCDATE());

-- ================================
-- ORGANIC HERBS & SPICES (5 items)
-- ================================
INSERT INTO Products (Id, Name, Slug, Description, Price, Stock, Unit, Sku, CategoryId, Brand, IsOrganic, Origin, FarmName, HarvestDate, BestBefore, CertificationNumber, CertificationType, CertifyingAgency, NutritionJson, SeoTitle, SeoDescription, IsActive, IsFeatured, CreatedAt) VALUES
(@P31, 'Organic Turmeric Powder', 'organic-turmeric-powder-lakadong', 'Premium Lakadong turmeric with high curcumin content. Known for its medicinal properties.', 29999, 80, '200g', 'HRB-TUR-001', @CatHerbs, 'Meghalaya Spices', 1, 'Meghalaya, India', 'Lakadong Valley Farm', DATEADD(day, -10, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-ML-3101', 'India Organic', 'APEDA', '{"calories":312,"protein":10,"carbs":67,"fiber":22,"curcumin":7.5,"iron":55}', 'Organic Lakadong Turmeric | High Curcumin', 'Premium turmeric with 7%+ curcumin content.', 1, 1, GETUTCDATE()),

(@P32, 'Organic Fresh Ginger', 'organic-fresh-ginger-cochin', 'Aromatic organic ginger from Cochin. Fresh, pungent, and perfect for cooking and tea.', 11999, 100, 'kg', 'HRB-GIN-001', @CatHerbs, 'Kerala Spice Garden', 1, 'Cochin, Kerala', 'Spice Trail Organics', DATEADD(day, -3, GETUTCDATE()), DATEADD(day, 21, GETUTCDATE()), 'IN-ORG-2024-KL-3201', 'India Organic', 'Ecocert', '{"calories":80,"protein":1.8,"carbs":18,"fiber":2,"gingerol":2.5,"vitaminB6":0.2}', 'Organic Fresh Ginger | Cochin', 'Aromatic fresh ginger from Kerala.', 1, 0, GETUTCDATE()),

(@P33, 'Organic Fresh Basil (Tulsi)', 'organic-fresh-basil-tulsi', 'Sacred tulsi leaves fresh from the farm. Perfect for tea, cooking, and Ayurvedic uses.', 4999, 60, 'bunch', 'HRB-BAS-001', @CatHerbs, 'Holy Herbs', 1, 'Varanasi, UP', 'Ganga Valley Organics', GETUTCDATE(), DATEADD(day, 5, GETUTCDATE()), 'IN-ORG-2024-UP-3301', 'India Organic', 'IMO Control', '{"calories":23,"protein":3.2,"carbs":2.7,"fiber":1.6,"vitaminK":415,"vitaminA":264}', 'Fresh Organic Tulsi | Holy Basil', 'Sacred tulsi leaves. Fresh and aromatic.', 1, 0, GETUTCDATE()),

(@P34, 'Organic Ceylon Cinnamon', 'organic-ceylon-cinnamon', 'True Ceylon cinnamon sticks. Sweet and delicate flavor, unlike cassia. Low in coumarin.', 24999, 45, '100g', 'HRB-CIN-001', @CatHerbs, 'Ceylon Imports', 1, 'Sri Lanka', 'Ceylon Spice Gardens', DATEADD(day, -15, GETUTCDATE()), DATEADD(day, 730, GETUTCDATE()), 'SL-ORG-2024-001', 'Sri Lanka Organic', 'Control Union', '{"calories":247,"protein":4,"carbs":81,"fiber":53,"calcium":1002,"manganese":17}', 'True Ceylon Cinnamon | Premium', 'Authentic Ceylon cinnamon sticks.', 1, 0, GETUTCDATE()),

(@P35, 'Organic Dried Oregano', 'organic-dried-oregano', 'Mediterranean organic dried oregano. Perfect for Italian dishes, pizzas, and salads.', 14999, 55, '50g', 'HRB-ORE-001', @CatHerbs, 'Mediterranean Herbs', 1, 'Ooty, Tamil Nadu', 'Nilgiri Herb Garden', DATEADD(day, -20, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-TN-3501', 'India Organic', 'Ecocert', '{"calories":265,"protein":9,"carbs":69,"fiber":43,"vitaminK":622,"iron":37}', 'Organic Dried Oregano | Italian', 'Premium dried oregano for cooking.', 1, 0, GETUTCDATE());

-- ==================================
-- ORGANIC LEGUMES & PULSES (5 items)
-- ==================================
INSERT INTO Products (Id, Name, Slug, Description, Price, Stock, Unit, Sku, CategoryId, Brand, IsOrganic, Origin, FarmName, HarvestDate, BestBefore, CertificationNumber, CertificationType, CertifyingAgency, NutritionJson, SeoTitle, SeoDescription, IsActive, IsFeatured, CreatedAt) VALUES
(@P36, 'Organic Chickpeas (Kabuli Chana)', 'organic-chickpeas-kabuli', 'Large white organic chickpeas. Perfect for hummus, curries, and salads.', 12999, 100, 'kg', 'LEG-CHK-001', @CatLegumes, 'Pulse Perfect', 1, 'Indore, MP', 'Central India Organics', DATEADD(day, -30, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-MP-3601', 'India Organic', 'APEDA', '{"calories":364,"protein":19,"carbs":61,"fiber":17,"iron":6.2,"folate":557}', 'Organic Kabuli Chickpeas | Protein Rich', 'Premium organic chickpeas for cooking.', 1, 1, GETUTCDATE()),

(@P37, 'Organic Masoor Dal (Red Lentils)', 'organic-masoor-dal', 'Quick-cooking organic red lentils. Staple protein source for everyday dal.', 9999, 150, 'kg', 'LEG-LEN-001', @CatLegumes, 'Dal House', 1, 'Raipur, Chhattisgarh', 'Heart of India Farms', DATEADD(day, -25, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-CG-3701', 'India Organic', 'IMO Control', '{"calories":352,"protein":25,"carbs":63,"fiber":11,"iron":6.5,"folate":479}', 'Organic Masoor Dal | Red Lentils', 'Quick-cooking organic red lentils.', 1, 0, GETUTCDATE()),

(@P38, 'Organic Black Beans (Rajma)', 'organic-black-rajma', 'Himalayan organic black rajma. Rich, creamy texture perfect for rajma chawal.', 14999, 80, 'kg', 'LEG-RAJ-001', @CatLegumes, 'Himalayan Pulses', 1, 'Chamba, HP', 'Himalayan Bean Farm', DATEADD(day, -40, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-HP-3801', 'India Organic', 'Control Union', '{"calories":341,"protein":21,"carbs":62,"fiber":16,"iron":5,"potassium":1406}', 'Organic Black Rajma | Himalayan', 'Premium black kidney beans from Himalayas.', 1, 0, GETUTCDATE()),

(@P39, 'Organic Kidney Beans (Red Rajma)', 'organic-red-rajma-jammu', 'Famous Jammu red rajma. Large, kidney-shaped beans with creamy texture.', 16999, 70, 'kg', 'LEG-KID-001', @CatLegumes, 'Jammu Organics', 1, 'Jammu, J&K', 'Tawi Valley Farms', DATEADD(day, -35, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-JK-3901', 'India Organic', 'APEDA', '{"calories":333,"protein":24,"carbs":60,"fiber":15,"iron":5.2,"folate":394}', 'Jammu Red Rajma | Organic', 'Famous Jammu kidney beans.', 1, 0, GETUTCDATE()),

(@P40, 'Organic Green Peas (Dried)', 'organic-green-peas-dried', 'Sweet organic dried green peas. Perfect for curries and winter soups.', 11999, 90, 'kg', 'LEG-PEA-001', @CatLegumes, 'Pea Valley', 1, 'Amritsar, Punjab', 'Golden Temple Organics', DATEADD(day, -45, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-PB-4001', 'India Organic', 'Ecocert', '{"calories":341,"protein":25,"carbs":60,"fiber":25,"iron":4.4,"vitaminC":40}', 'Organic Green Peas | Dried', 'Sweet organic dried green peas.', 1, 0, GETUTCDATE());

-- ==============================
-- ORGANIC NUTS & SEEDS (5 items)
-- ==============================
INSERT INTO Products (Id, Name, Slug, Description, Price, Stock, Unit, Sku, CategoryId, Brand, IsOrganic, Origin, FarmName, HarvestDate, BestBefore, CertificationNumber, CertificationType, CertifyingAgency, NutritionJson, SeoTitle, SeoDescription, IsActive, IsFeatured, CreatedAt) VALUES
(@P41, 'Organic Almonds (Mamra)', 'organic-mamra-almonds-kashmir', 'Premium Kashmiri Mamra almonds. Naturally sweet with high oil content. Superior taste.', 149999, 30, 'kg', 'NUT-ALM-001', @CatNuts, 'Kashmir Valley', 1, 'Srinagar, J&K', 'Paradise Nut Farm', DATEADD(day, -60, GETUTCDATE()), DATEADD(day, 180, GETUTCDATE()), 'IN-ORG-2024-JK-4101', 'India Organic', 'APEDA', '{"calories":579,"protein":21,"carbs":22,"fiber":12,"fat":50,"vitaminE":25}', 'Kashmiri Mamra Almonds | Premium', 'Premium organic Mamra almonds from Kashmir.', 1, 1, GETUTCDATE()),

(@P42, 'Organic Walnuts (Akhrot)', 'organic-walnuts-kashmir', 'Brain-shaped organic walnuts from Kashmir. Rich in omega-3 and perfect for brain health.', 99999, 40, 'kg', 'NUT-WAL-001', @CatNuts, 'Kashmir Dryfruits', 1, 'Anantnag, J&K', 'Walnut Grove Organics', DATEADD(day, -50, GETUTCDATE()), DATEADD(day, 180, GETUTCDATE()), 'IN-ORG-2024-JK-4201', 'India Organic', 'IMO Control', '{"calories":654,"protein":15,"carbs":14,"fiber":7,"fat":65,"omega3":9}', 'Kashmir Organic Walnuts | Brain Food', 'Omega-3 rich organic walnuts.', 1, 1, GETUTCDATE()),

(@P43, 'Organic Chia Seeds', 'organic-chia-seeds', 'Tiny but mighty organic chia seeds. Packed with omega-3, fiber, and protein.', 39999, 60, '500g', 'NUT-CHI-001', @CatNuts, 'Superfood Nation', 1, 'Rajasthan, India', 'Desert Organic Farm', DATEADD(day, -30, GETUTCDATE()), DATEADD(day, 730, GETUTCDATE()), 'IN-ORG-2024-RJ-4301', 'India Organic', 'Ecocert', '{"calories":486,"protein":17,"carbs":42,"fiber":34,"fat":31,"omega3":18}', 'Organic Chia Seeds | Superfood', 'Nutrient-dense organic chia seeds.', 1, 0, GETUTCDATE()),

(@P44, 'Organic Flaxseeds (Alsi)', 'organic-flaxseeds-alsi', 'Golden organic flaxseeds. Rich in lignans and omega-3. Heart-healthy superfood.', 24999, 80, '500g', 'NUT-FLX-001', @CatNuts, 'Omega Organics', 1, 'Madhya Pradesh, India', 'Golden Seed Farm', DATEADD(day, -20, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-MP-4401', 'India Organic', 'Control Union', '{"calories":534,"protein":18,"carbs":29,"fiber":27,"fat":42,"omega3":23}', 'Organic Flaxseeds | Heart Healthy', 'Golden organic flaxseeds.', 1, 0, GETUTCDATE()),

(@P45, 'Organic Pumpkin Seeds', 'organic-pumpkin-seeds', 'Crunchy organic pumpkin seeds. Rich in zinc and magnesium. Perfect for snacking.', 34999, 50, '500g', 'NUT-PMP-001', @CatNuts, 'Seed Store', 1, 'Rajasthan, India', 'Thar Organic Seeds', DATEADD(day, -25, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-RJ-4501', 'India Organic', 'APEDA', '{"calories":559,"protein":30,"carbs":11,"fiber":6,"fat":49,"zinc":8,"magnesium":550}', 'Organic Pumpkin Seeds | Zinc Rich', 'Crunchy organic pumpkin seeds.', 1, 0, GETUTCDATE());

-- =============================
-- ORGANIC BEVERAGES (5 items)
-- =============================
INSERT INTO Products (Id, Name, Slug, Description, Price, Stock, Unit, Sku, CategoryId, Brand, IsOrganic, Origin, FarmName, HarvestDate, BestBefore, CertificationNumber, CertificationType, CertifyingAgency, NutritionJson, SeoTitle, SeoDescription, IsActive, IsFeatured, CreatedAt) VALUES
(@P46, 'Organic Green Tea', 'organic-green-tea-darjeeling', 'Premium Darjeeling organic green tea. Light, refreshing, and rich in antioxidants.', 29999, 70, '100g', 'BEV-GRT-001', @CatBeverages, 'Darjeeling Tea Estate', 1, 'Darjeeling, WB', 'Happy Valley Tea Estate', DATEADD(day, -15, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-WB-4601', 'India Organic', 'IMO Control', '{"calories":0,"caffeine":25,"catechins":150,"egcg":70}', 'Darjeeling Organic Green Tea', 'Premium organic green tea from Darjeeling.', 1, 1, GETUTCDATE()),

(@P47, 'Organic Chamomile Tea', 'organic-chamomile-tea', 'Calming organic chamomile flowers. Perfect for relaxation and better sleep.', 24999, 50, '50g', 'BEV-CHM-001', @CatBeverages, 'Herbal Haven', 1, 'Uttarakhand, India', 'Mountain Herb Farm', DATEADD(day, -10, GETUTCDATE()), DATEADD(day, 365, GETUTCDATE()), 'IN-ORG-2024-UK-4701', 'India Organic', 'Ecocert', '{"calories":0,"caffeine":0,"flavonoids":true}', 'Organic Chamomile Tea | Sleep Aid', 'Relaxing organic chamomile tea.', 1, 0, GETUTCDATE()),

(@P48, 'Organic Coconut Water', 'organic-coconut-water-tender', 'Fresh organic tender coconut water. Natural electrolytes and hydration.', 4999, 100, '300ml', 'BEV-COC-001', @CatBeverages, 'Kerala Coco', 1, 'Kochi, Kerala', 'Coconut Paradise', GETUTCDATE(), DATEADD(day, 3, GETUTCDATE()), 'IN-ORG-2024-KL-4801', 'India Organic', 'APEDA', '{"calories":19,"potassium":250,"sodium":105,"magnesium":25}', 'Fresh Organic Coconut Water', 'Natural hydration from Kerala coconuts.', 1, 1, GETUTCDATE()),

(@P49, 'Organic Kombucha - Ginger Lemon', 'organic-kombucha-ginger-lemon', 'Fermented organic kombucha with ginger and lemon. Probiotic-rich and refreshing.', 19999, 35, '350ml', 'BEV-KOM-001', @CatBeverages, 'Ferment Co', 1, 'Bangalore, Karnataka', 'Culture Craft Brewery', DATEADD(day, -5, GETUTCDATE()), DATEADD(day, 30, GETUTCDATE()), 'IN-ORG-2024-KA-4901', 'India Organic', 'Control Union', '{"calories":30,"sugar":6,"probiotics":true,"scobies":true}', 'Organic Kombucha | Ginger Lemon', 'Probiotic-rich fermented tea.', 1, 0, GETUTCDATE()),

(@P50, 'Organic Mixed Vegetable Juice', 'organic-vegetable-juice-mixed', 'Cold-pressed organic vegetable juice. Blend of carrot, beet, spinach, and ginger.', 14999, 25, '500ml', 'BEV-VEG-001', @CatBeverages, 'Fresh Press', 1, 'Pune, Maharashtra', 'Juice Lab Organics', GETUTCDATE(), DATEADD(day, 3, GETUTCDATE()), 'IN-ORG-2024-MH-5001', 'India Organic', 'Ecocert', '{"calories":60,"vitaminA":200,"vitaminC":30,"iron":2,"fiber":2}', 'Organic Cold-Pressed Vegetable Juice', 'Nutrient-packed organic veggie juice.', 1, 0, GETUTCDATE());

-- ============================================================================
-- STEP 5: Create ProductImages
-- ============================================================================

-- FRUITS IMAGES
INSERT INTO ProductImages (Id, ProductId, Url, AltText, SortOrder, IsPrimary, CreatedAt) VALUES
(NEWID(), @P1, '/product-images/organic-fruits/bananas/1.jpg', 'Organic Bananas - Fresh bunch', 0, 1, GETUTCDATE()),
(NEWID(), @P1, '/product-images/organic-fruits/bananas/2.jpg', 'Organic Bananas - Close up', 1, 0, GETUTCDATE()),
(NEWID(), @P1, '/product-images/organic-fruits/bananas/3.jpg', 'Organic Bananas - Farm fresh', 2, 0, GETUTCDATE()),

(NEWID(), @P2, '/product-images/organic-fruits/apples/3.jpg', 'Himalayan Organic Apples - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P2, '/product-images/organic-fruits/apples/4.jpg', 'Himalayan Apples - Red variety', 1, 0, GETUTCDATE()),

(NEWID(), @P3, '/product-images/organic-fruits/mangoes/1.jpg', 'Alphonso Mangoes - King of fruits', 0, 1, GETUTCDATE()),
(NEWID(), @P3, '/product-images/organic-fruits/mangoes/3.jpg', 'Alphonso Mangoes - Ripe', 1, 0, GETUTCDATE()),

(NEWID(), @P4, '/product-images/organic-fruits/oranges/1.jpg', 'Nagpur Oranges - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P4, '/product-images/organic-fruits/oranges/2.jpg', 'Nagpur Oranges - Juicy', 1, 0, GETUTCDATE()),

(NEWID(), @P5, '/product-images/organic-fruits/strawberries/1.jpg', 'Organic Strawberries - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P5, '/product-images/organic-fruits/strawberries/2.jpg', 'Organic Strawberries - Ripe red', 1, 0, GETUTCDATE()),

(NEWID(), @P6, '/product-images/organic-fruits/blueberries/1.jpg', 'Organic Blueberries - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P6, '/product-images/organic-fruits/blueberries/2.jpg', 'Organic Blueberries - Close up', 1, 0, GETUTCDATE()),

(NEWID(), @P7, '/product-images/organic-fruits/avocados/1.jpg', 'Organic Avocados - Ripe', 0, 1, GETUTCDATE()),
(NEWID(), @P7, '/product-images/organic-fruits/avocados/2.jpg', 'Organic Avocados - Cut open', 1, 0, GETUTCDATE()),

(NEWID(), @P8, '/product-images/organic-fruits/grapes/1.jpg', 'Red Grapes - Fresh bunch', 0, 1, GETUTCDATE()),
(NEWID(), @P8, '/product-images/organic-fruits/grapes/2.jpg', 'Red Grapes - Vineyard fresh', 1, 0, GETUTCDATE()),

(NEWID(), @P9, '/product-images/organic-fruits/pomegranates/1.jpg', 'Organic Pomegranates - Whole', 0, 1, GETUTCDATE()),
(NEWID(), @P9, '/product-images/organic-fruits/pomegranates/2.jpg', 'Organic Pomegranates - Seeds', 1, 0, GETUTCDATE()),

(NEWID(), @P10, '/product-images/organic-fruits/raspberries/1.jpg', 'Organic Raspberries - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P10, '/product-images/organic-fruits/raspberries/2.jpg', 'Organic Raspberries - Close up', 1, 0, GETUTCDATE());

-- VEGETABLES IMAGES
INSERT INTO ProductImages (Id, ProductId, Url, AltText, SortOrder, IsPrimary, CreatedAt) VALUES
(NEWID(), @P11, '/product-images/organic-vegetables/tomatoes/1.jpg', 'Organic Tomatoes - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P11, '/product-images/organic-vegetables/tomatoes/2.jpg', 'Organic Tomatoes - Vine ripened', 1, 0, GETUTCDATE()),

(NEWID(), @P12, '/product-images/organic-vegetables/spinach/2.jpg', 'Baby Spinach - Fresh leaves', 0, 1, GETUTCDATE()),
(NEWID(), @P12, '/product-images/organic-vegetables/spinach/3.jpg', 'Baby Spinach - Bundle', 1, 0, GETUTCDATE()),

(NEWID(), @P13, '/product-images/organic-vegetables/carrots/1.jpg', 'Organic Carrots - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P13, '/product-images/organic-vegetables/carrots/2.jpg', 'Organic Carrots - Bundle', 1, 0, GETUTCDATE()),

(NEWID(), @P14, '/product-images/organic-vegetables/broccoli/1.jpg', 'Organic Broccoli - Fresh florets', 0, 1, GETUTCDATE()),
(NEWID(), @P14, '/product-images/organic-vegetables/broccoli/2.jpg', 'Organic Broccoli - Close up', 1, 0, GETUTCDATE()),

(NEWID(), @P15, '/product-images/organic-vegetables/kale/1.jpg', 'Organic Kale - Fresh leaves', 0, 1, GETUTCDATE()),
(NEWID(), @P15, '/product-images/organic-vegetables/kale/2.jpg', 'Organic Kale - Bundle', 1, 0, GETUTCDATE()),

(NEWID(), @P16, '/product-images/organic-vegetables/bell-peppers/1.jpg', 'Mixed Bell Peppers - Colorful', 0, 1, GETUTCDATE()),
(NEWID(), @P16, '/product-images/organic-vegetables/bell-peppers/3.jpg', 'Bell Peppers - Fresh', 1, 0, GETUTCDATE()),

(NEWID(), @P17, '/product-images/organic-vegetables/sweet-potato/1.jpg', 'Organic Sweet Potatoes - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P17, '/product-images/organic-vegetables/sweet-potato/2.jpg', 'Sweet Potatoes - Cut open', 1, 0, GETUTCDATE()),

(NEWID(), @P18, '/product-images/organic-vegetables/cauliflower/1.jpg', 'Organic Cauliflower - Fresh head', 0, 1, GETUTCDATE()),
(NEWID(), @P18, '/product-images/organic-vegetables/cauliflower/2.jpg', 'Cauliflower - Florets', 1, 0, GETUTCDATE()),

(NEWID(), @P19, '/product-images/organic-vegetables/beets/1.jpg', 'Organic Beetroot - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P19, '/product-images/organic-vegetables/beets/2.jpg', 'Beetroot - Cut open', 1, 0, GETUTCDATE()),

(NEWID(), @P20, '/product-images/organic-vegetables/brussels-sprouts/1.jpg', 'Brussels Sprouts - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P20, '/product-images/organic-vegetables/brussels-sprouts/2.jpg', 'Brussels Sprouts - Close up', 1, 0, GETUTCDATE());

-- DAIRY IMAGES
INSERT INTO ProductImages (Id, ProductId, Url, AltText, SortOrder, IsPrimary, CreatedAt) VALUES
(NEWID(), @P21, '/product-images/organic-dairy-products/milk/1.jpg', 'Organic A2 Milk - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P21, '/product-images/organic-dairy-products/milk/2.jpg', 'A2 Milk - Gir cow', 1, 0, GETUTCDATE()),

(NEWID(), @P22, '/product-images/organic-dairy-products/greek-yogurt/1.jpg', 'Greek Yogurt - Creamy', 0, 1, GETUTCDATE()),
(NEWID(), @P22, '/product-images/organic-dairy-products/greek-yogurt/2.jpg', 'Greek Yogurt - Thick', 1, 0, GETUTCDATE()),

(NEWID(), @P23, '/product-images/organic-dairy-products/cottage-cheese/1.jpg', 'Organic Paneer - Fresh', 0, 1, GETUTCDATE()),
(NEWID(), @P23, '/product-images/organic-dairy-products/cottage-cheese/2.jpg', 'Paneer - Cubed', 1, 0, GETUTCDATE()),

(NEWID(), @P24, '/product-images/organic-dairy-products/cheddar-cheese/1.jpg', 'Aged Cheddar - Block', 0, 1, GETUTCDATE()),
(NEWID(), @P24, '/product-images/organic-dairy-products/cheddar-cheese/2.jpg', 'Cheddar - Sliced', 1, 0, GETUTCDATE()),

(NEWID(), @P25, '/product-images/organic-dairy-products/kefir/1.jpg', 'Organic Kefir - Probiotic', 0, 1, GETUTCDATE()),
(NEWID(), @P25, '/product-images/organic-dairy-products/kefir/2.jpg', 'Kefir - Bottle', 1, 0, GETUTCDATE());

-- GRAINS IMAGES
INSERT INTO ProductImages (Id, ProductId, Url, AltText, SortOrder, IsPrimary, CreatedAt) VALUES
(NEWID(), @P26, '/product-images/organic-grains-cereals/brown-rice/1.jpg', 'Organic Brown Rice - Premium', 0, 1, GETUTCDATE()),
(NEWID(), @P26, '/product-images/organic-grains-cereals/brown-rice/2.jpg', 'Brown Rice - Grains', 1, 0, GETUTCDATE()),

(NEWID(), @P27, '/product-images/organic-grains-cereals/quinoa/1.jpg', 'Organic Quinoa - White', 0, 1, GETUTCDATE()),
(NEWID(), @P27, '/product-images/organic-grains-cereals/quinoa/2.jpg', 'Quinoa - Cooked', 1, 0, GETUTCDATE()),

(NEWID(), @P28, '/product-images/organic-grains-cereals/oats/1.jpg', 'Organic Rolled Oats', 0, 1, GETUTCDATE()),
(NEWID(), @P28, '/product-images/organic-grains-cereals/oats/3.jpg', 'Oats - Bowl', 1, 0, GETUTCDATE()),

(NEWID(), @P29, '/product-images/organic-grains-cereals/millet/1.jpg', 'Organic Ragi - Finger millet', 0, 1, GETUTCDATE()),
(NEWID(), @P29, '/product-images/organic-grains-cereals/millet/2.jpg', 'Ragi - Flour', 1, 0, GETUTCDATE()),

(NEWID(), @P30, '/product-images/organic-grains-cereals/buckwheat/1.jpg', 'Organic Buckwheat', 0, 1, GETUTCDATE()),
(NEWID(), @P30, '/product-images/organic-grains-cereals/buckwheat/2.jpg', 'Buckwheat - Grains', 1, 0, GETUTCDATE());

-- HERBS & SPICES IMAGES
INSERT INTO ProductImages (Id, ProductId, Url, AltText, SortOrder, IsPrimary, CreatedAt) VALUES
(NEWID(), @P31, '/product-images/organic-herbs-spices/turmeric/1.jpg', 'Organic Turmeric - Lakadong', 0, 1, GETUTCDATE()),
(NEWID(), @P31, '/product-images/organic-herbs-spices/turmeric/2.jpg', 'Turmeric - Powder', 1, 0, GETUTCDATE()),

(NEWID(), @P32, '/product-images/organic-herbs-spices/ginger/1.jpg', 'Fresh Organic Ginger', 0, 1, GETUTCDATE()),
(NEWID(), @P32, '/product-images/organic-herbs-spices/ginger/2.jpg', 'Ginger - Cut', 1, 0, GETUTCDATE()),

(NEWID(), @P33, '/product-images/organic-herbs-spices/basil/1.jpg', 'Fresh Tulsi - Holy Basil', 0, 1, GETUTCDATE()),
(NEWID(), @P33, '/product-images/organic-herbs-spices/basil/2.jpg', 'Tulsi - Leaves', 1, 0, GETUTCDATE()),

(NEWID(), @P34, '/product-images/organic-herbs-spices/cinnamon/1.jpg', 'Ceylon Cinnamon - Sticks', 0, 1, GETUTCDATE()),
(NEWID(), @P34, '/product-images/organic-herbs-spices/cinnamon/2.jpg', 'Cinnamon - Rolled', 1, 0, GETUTCDATE()),

(NEWID(), @P35, '/product-images/organic-herbs-spices/oregano/1.jpg', 'Dried Oregano - Organic', 0, 1, GETUTCDATE()),
(NEWID(), @P35, '/product-images/organic-herbs-spices/oregano/2.jpg', 'Oregano - Jar', 1, 0, GETUTCDATE());

-- LEGUMES & PULSES IMAGES
INSERT INTO ProductImages (Id, ProductId, Url, AltText, SortOrder, IsPrimary, CreatedAt) VALUES
(NEWID(), @P36, '/product-images/organic-legumes-pulses/chickpeas/1.jpg', 'Organic Chickpeas - Kabuli', 0, 1, GETUTCDATE()),
(NEWID(), @P36, '/product-images/organic-legumes-pulses/chickpeas/2.jpg', 'Chickpeas - Close up', 1, 0, GETUTCDATE()),

(NEWID(), @P37, '/product-images/organic-legumes-pulses/lentils/1.jpg', 'Masoor Dal - Red Lentils', 0, 1, GETUTCDATE()),
(NEWID(), @P37, '/product-images/organic-legumes-pulses/lentils/2.jpg', 'Lentils - Bowl', 1, 0, GETUTCDATE()),

(NEWID(), @P38, '/product-images/organic-legumes-pulses/black-beans/1.jpg', 'Black Rajma - Organic', 0, 1, GETUTCDATE()),
(NEWID(), @P38, '/product-images/organic-legumes-pulses/black-beans/2.jpg', 'Black Beans - Close up', 1, 0, GETUTCDATE()),

(NEWID(), @P39, '/product-images/organic-legumes-pulses/kidney-beans/1.jpg', 'Red Rajma - Jammu', 0, 1, GETUTCDATE()),
(NEWID(), @P39, '/product-images/organic-legumes-pulses/kidney-beans/2.jpg', 'Kidney Beans - Close up', 1, 0, GETUTCDATE()),

(NEWID(), @P40, '/product-images/organic-legumes-pulses/green-peas/1.jpg', 'Organic Green Peas - Dried', 0, 1, GETUTCDATE()),
(NEWID(), @P40, '/product-images/organic-legumes-pulses/green-peas/2.jpg', 'Green Peas - Close up', 1, 0, GETUTCDATE());

-- NUTS & SEEDS IMAGES
INSERT INTO ProductImages (Id, ProductId, Url, AltText, SortOrder, IsPrimary, CreatedAt) VALUES
(NEWID(), @P41, '/product-images/organic-nuts-seeds/almonds/1.jpg', 'Mamra Almonds - Kashmir', 0, 1, GETUTCDATE()),
(NEWID(), @P41, '/product-images/organic-nuts-seeds/almonds/2.jpg', 'Almonds - Premium', 1, 0, GETUTCDATE()),

(NEWID(), @P42, '/product-images/organic-nuts-seeds/walnuts/1.jpg', 'Kashmir Walnuts - Organic', 0, 1, GETUTCDATE()),
(NEWID(), @P42, '/product-images/organic-nuts-seeds/walnuts/3.jpg', 'Walnuts - Halves', 1, 0, GETUTCDATE()),

(NEWID(), @P43, '/product-images/organic-nuts-seeds/chia-seeds/1.jpg', 'Organic Chia Seeds', 0, 1, GETUTCDATE()),
(NEWID(), @P43, '/product-images/organic-nuts-seeds/chia-seeds/2.jpg', 'Chia Seeds - Close up', 1, 0, GETUTCDATE()),

(NEWID(), @P44, '/product-images/organic-nuts-seeds/flaxseeds/1.jpg', 'Organic Flaxseeds - Golden', 0, 1, GETUTCDATE()),
(NEWID(), @P44, '/product-images/organic-nuts-seeds/flaxseeds/3.jpg', 'Flaxseeds - Close up', 1, 0, GETUTCDATE()),

(NEWID(), @P45, '/product-images/organic-nuts-seeds/pumpkin-seeds/1.jpg', 'Organic Pumpkin Seeds', 0, 1, GETUTCDATE()),
(NEWID(), @P45, '/product-images/organic-nuts-seeds/pumpkin-seeds/2.jpg', 'Pumpkin Seeds - Close up', 1, 0, GETUTCDATE());

-- BEVERAGES IMAGES
INSERT INTO ProductImages (Id, ProductId, Url, AltText, SortOrder, IsPrimary, CreatedAt) VALUES
(NEWID(), @P46, '/product-images/organic-beverages/green-tea/1.jpg', 'Darjeeling Green Tea', 0, 1, GETUTCDATE()),
(NEWID(), @P46, '/product-images/organic-beverages/green-tea/2.jpg', 'Green Tea - Leaves', 1, 0, GETUTCDATE()),

(NEWID(), @P47, '/product-images/organic-beverages/chamomile-tea/1.png', 'Organic Chamomile Tea', 0, 1, GETUTCDATE()),
(NEWID(), @P47, '/product-images/organic-beverages/chamomile-tea/3.jpg', 'Chamomile - Flowers', 1, 0, GETUTCDATE()),

(NEWID(), @P48, '/product-images/organic-beverages/coconut-water/1.jpg', 'Fresh Coconut Water', 0, 1, GETUTCDATE()),
(NEWID(), @P48, '/product-images/organic-beverages/coconut-water/2.jpg', 'Coconut Water - Tender', 1, 0, GETUTCDATE()),

(NEWID(), @P49, '/product-images/organic-beverages/kombucha/1.png', 'Organic Kombucha - Ginger Lemon', 0, 1, GETUTCDATE()),
(NEWID(), @P49, '/product-images/organic-beverages/kombucha/2.jpg', 'Kombucha - Bottle', 1, 0, GETUTCDATE()),

(NEWID(), @P50, '/product-images/organic-beverages/vegetable-juice/2.jpg', 'Mixed Vegetable Juice', 0, 1, GETUTCDATE()),
(NEWID(), @P50, '/product-images/organic-beverages/vegetable-juice/3.jpg', 'Veggie Juice - Glass', 1, 0, GETUTCDATE());

-- ============================================================================
-- STEP 6: Create ProductTags (Many-to-Many associations)
-- ============================================================================

-- FRUITS Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(@P1, @TagOrganic), (@P1, @TagLocal), (@P1, @TagFarmFresh), (@P1, @TagBestSeller),
(@P2, @TagOrganic), (@P2, @TagLocal), (@P2, @TagSeasonal), (@P2, @TagBestSeller),
(@P3, @TagOrganic), (@P3, @TagSeasonal), (@P3, @TagBestSeller), (@P3, @TagNewArrival),
(@P4, @TagOrganic), (@P4, @TagLocal), (@P4, @TagFarmFresh),
(@P5, @TagOrganic), (@P5, @TagSeasonal), (@P5, @TagFarmFresh), (@P5, @TagNewArrival),
(@P6, @TagOrganic), (@P6, @TagSuperFood), (@P6, @TagPesticideFree),
(@P7, @TagOrganic), (@P7, @TagSuperFood), (@P7, @TagHighProtein),
(@P8, @TagOrganic), (@P8, @TagLocal), (@P8, @TagSeasonal),
(@P9, @TagOrganic), (@P9, @TagSuperFood), (@P9, @TagBestSeller),
(@P10, @TagOrganic), (@P10, @TagSeasonal), (@P10, @TagPesticideFree);

-- VEGETABLES Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(@P11, @TagOrganic), (@P11, @TagLocal), (@P11, @TagFarmFresh), (@P11, @TagBestSeller),
(@P12, @TagOrganic), (@P12, @TagSuperFood), (@P12, @TagVegan), (@P12, @TagBestSeller),
(@P13, @TagOrganic), (@P13, @TagLocal), (@P13, @TagFarmFresh),
(@P14, @TagOrganic), (@P14, @TagSuperFood), (@P14, @TagVegan),
(@P15, @TagOrganic), (@P15, @TagSuperFood), (@P15, @TagVegan), (@P15, @TagBestSeller),
(@P16, @TagOrganic), (@P16, @TagLocal), (@P16, @TagFarmFresh),
(@P17, @TagOrganic), (@P17, @TagLocal), (@P17, @TagGlutenFree),
(@P18, @TagOrganic), (@P18, @TagLocal), (@P18, @TagFarmFresh),
(@P19, @TagOrganic), (@P19, @TagSuperFood), (@P19, @TagVegan),
(@P20, @TagOrganic), (@P20, @TagSuperFood), (@P20, @TagVegan);

-- DAIRY Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(@P21, @TagOrganic), (@P21, @TagLocal), (@P21, @TagHighProtein), (@P21, @TagBestSeller),
(@P22, @TagOrganic), (@P22, @TagHighProtein), (@P22, @TagBestSeller),
(@P23, @TagOrganic), (@P23, @TagLocal), (@P23, @TagHighProtein),
(@P24, @TagOrganic), (@P24, @TagHighProtein),
(@P25, @TagOrganic), (@P25, @TagSuperFood);

-- GRAINS Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(@P26, @TagOrganic), (@P26, @TagGlutenFree), (@P26, @TagVegan), (@P26, @TagBestSeller),
(@P27, @TagOrganic), (@P27, @TagSuperFood), (@P27, @TagGlutenFree), (@P27, @TagHighProtein), (@P27, @TagBestSeller),
(@P28, @TagOrganic), (@P28, @TagGlutenFree), (@P28, @TagVegan),
(@P29, @TagOrganic), (@P29, @TagGlutenFree), (@P29, @TagLocal),
(@P30, @TagOrganic), (@P30, @TagGlutenFree), (@P30, @TagVegan);

-- HERBS & SPICES Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(@P31, @TagOrganic), (@P31, @TagSuperFood), (@P31, @TagLocal), (@P31, @TagBestSeller),
(@P32, @TagOrganic), (@P32, @TagLocal), (@P32, @TagFarmFresh),
(@P33, @TagOrganic), (@P33, @TagLocal), (@P33, @TagFarmFresh),
(@P34, @TagOrganic), (@P34, @TagSuperFood),
(@P35, @TagOrganic), (@P35, @TagVegan);

-- LEGUMES & PULSES Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(@P36, @TagOrganic), (@P36, @TagHighProtein), (@P36, @TagVegan), (@P36, @TagBestSeller),
(@P37, @TagOrganic), (@P37, @TagHighProtein), (@P37, @TagVegan),
(@P38, @TagOrganic), (@P38, @TagHighProtein), (@P38, @TagLocal),
(@P39, @TagOrganic), (@P39, @TagHighProtein), (@P39, @TagLocal),
(@P40, @TagOrganic), (@P40, @TagHighProtein), (@P40, @TagVegan);

-- NUTS & SEEDS Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(@P41, @TagOrganic), (@P41, @TagSuperFood), (@P41, @TagLocal), (@P41, @TagBestSeller),
(@P42, @TagOrganic), (@P42, @TagSuperFood), (@P42, @TagLocal), (@P42, @TagBestSeller),
(@P43, @TagOrganic), (@P43, @TagSuperFood), (@P43, @TagGlutenFree), (@P43, @TagVegan),
(@P44, @TagOrganic), (@P44, @TagSuperFood), (@P44, @TagGlutenFree), (@P44, @TagVegan),
(@P45, @TagOrganic), (@P45, @TagSuperFood), (@P45, @TagGlutenFree), (@P45, @TagVegan);

-- BEVERAGES Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(@P46, @TagOrganic), (@P46, @TagSuperFood), (@P46, @TagLocal), (@P46, @TagBestSeller),
(@P47, @TagOrganic), (@P47, @TagVegan), (@P47, @TagGlutenFree),
(@P48, @TagOrganic), (@P48, @TagLocal), (@P48, @TagVegan), (@P48, @TagBestSeller),
(@P49, @TagOrganic), (@P49, @TagSuperFood), (@P49, @TagVegan),
(@P50, @TagOrganic), (@P50, @TagVegan), (@P50, @TagSuperFood);

-- ============================================================================
-- VERIFICATION QUERIES
-- ============================================================================

-- Count products by category
SELECT c.Name AS Category, COUNT(p.Id) AS ProductCount
FROM Categories c
LEFT JOIN Products p ON p.CategoryId = c.Id
GROUP BY c.Name
ORDER BY c.Name;

-- Featured products
SELECT Name, Slug, Price, Stock, Origin, IsFeatured
FROM Products
WHERE IsFeatured = 1
ORDER BY Name;

-- Low stock items (< 30)
SELECT Name, Stock, Unit, BestBefore
FROM Products
WHERE Stock < 30
ORDER BY Stock;

-- Products expiring soon (within 7 days)
SELECT Name, BestBefore, DATEDIFF(day, GETUTCDATE(), BestBefore) AS DaysLeft
FROM Products
WHERE BestBefore IS NOT NULL AND DATEDIFF(day, GETUTCDATE(), BestBefore) <= 7
ORDER BY BestBefore;

PRINT 'Seed data inserted successfully!';
PRINT '50 Products across 8 Categories';
PRINT '12 Tags for product discovery';
PRINT '100+ Product Images';
GO
