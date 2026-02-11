SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
BEGIN TRY
  BEGIN TRANSACTION;

  -- Clean existing data (safe to rerun)
  DELETE FROM [ProductTags];
  DELETE FROM [ProductAttributes];
  DELETE FROM [ProductImages];
  DELETE FROM [ProductMetadata];
  DELETE FROM [ProductCertifications];
  DELETE FROM [Products];
  DELETE FROM [Tags];
  DELETE FROM [Categories];

  -- Tags
  INSERT INTO [Tags] ([Id],[Name],[Slug]) VALUES
    ('8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0','Organic','organic'),
    ('de7ce5a0-7d59-4979-bcc9-ed78bdfca273','Seasonal','seasonal'),
    ('bf80a8f8-280a-4b3c-ad79-32548705e17c','Farm-trust','farm-trust');

  -- Categories
  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('7f5f0873-97ca-4a8e-a27e-a22e9c0a3eb8','Organic Beverages','organic-beverages',NULL,NULL,1,SYSUTCDATETIME(),NULL);
  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('9a5bbc16-ec30-4c87-b0d3-e9b4778e66a2','Organic Dairy Products','organic-dairy-products',NULL,NULL,1,SYSUTCDATETIME(),NULL);
  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('3c5f7f01-c2f2-4142-8532-da208cfa29de','Organic Fruits','organic-fruits',NULL,NULL,1,SYSUTCDATETIME(),NULL);
  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('a33a5368-518b-4ed2-895e-0c71a5a81670','Organic Grains & Cereals','organic-grains-cereals',NULL,NULL,1,SYSUTCDATETIME(),NULL);
  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('14b7e158-ae37-4441-8868-97d15ff787d4','Organic Herbs & Spices','organic-herbs-spices',NULL,NULL,1,SYSUTCDATETIME(),NULL);
  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('75e9f924-74bc-4b9f-9271-71ff69a6dab1','Organic Legumes & Pulses','organic-legumes-pulses',NULL,NULL,1,SYSUTCDATETIME(),NULL);
  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('b410d77f-f06c-4223-8c8e-d626f65fdd20','Organic Nuts & Seeds','organic-nuts-seeds',NULL,NULL,1,SYSUTCDATETIME(),NULL);
  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('954671e4-0b9f-477b-8ee8-e2f0ba87138a','Organic Vegetables','organic-vegetables',NULL,NULL,1,SYSUTCDATETIME(),NULL);

  -- Products + Metadata
  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('4947208a-334e-4373-ba27-cbcab282bc6c','Chamomile tea','Fresh, organically grown Chamomile tea.',177,51,'7f5f0873-97ca-4a8e-a27e-a22e9c0a3eb8','FreshHarvest Market Organics','EP-ORG-CHAMOM','bottle',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('916a554d-a139-435a-9749-ccf7188be790','4947208a-334e-4373-ba27-cbcab282bc6c','organic-beverages-chamomile-tea','{"title":"Chamomile tea","description":"Fresh, organically grown Chamomile tea.","keywords":"Chamomile tea,Organic Beverages,organic,fresh","canonicalUrl":"/products/organic-beverages-chamomile-tea"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Chamomile tea
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4947208a-334e-4373-ba27-cbcab282bc6c','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4947208a-334e-4373-ba27-cbcab282bc6c','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4947208a-334e-4373-ba27-cbcab282bc6c','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','Coconut water','Fresh, organically grown Coconut water.',336,58,'7f5f0873-97ca-4a8e-a27e-a22e9c0a3eb8','FreshHarvest Market Organics','EP-ORG-COCONU','bottle',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('d919c000-8bf6-45a6-84a6-aaa34d0e1323','22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','organic-beverages-coconut-water','{"title":"Coconut water","description":"Fresh, organically grown Coconut water.","keywords":"Coconut water,Organic Beverages,organic,fresh","canonicalUrl":"/products/organic-beverages-coconut-water"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Coconut water
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('8f497f77-77b9-47c5-954d-a8bac6893d4b','Green tea','Fresh, organically grown Green tea.',159,63,'7f5f0873-97ca-4a8e-a27e-a22e9c0a3eb8','FreshHarvest Market Organics','EP-ORG-GREEN-','bottle',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('20e4bf9f-7f02-4ea6-8fa5-f83fe9602350','8f497f77-77b9-47c5-954d-a8bac6893d4b','organic-beverages-green-tea','{"title":"Green tea","description":"Fresh, organically grown Green tea.","keywords":"Green tea,Organic Beverages,organic,fresh","canonicalUrl":"/products/organic-beverages-green-tea"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Green tea
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('8f497f77-77b9-47c5-954d-a8bac6893d4b','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('8f497f77-77b9-47c5-954d-a8bac6893d4b','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('8f497f77-77b9-47c5-954d-a8bac6893d4b','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('6a8940b9-fd89-49de-bf33-ac2b5a7368b4','Kombucha','Fresh, organically grown Kombucha.',294,60,'7f5f0873-97ca-4a8e-a27e-a22e9c0a3eb8','FreshHarvest Market Organics','EP-ORG-KOMBUC','bottle',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('5feaa9e7-f8c6-4905-a8f4-95238af01927','6a8940b9-fd89-49de-bf33-ac2b5a7368b4','organic-beverages-kombucha','{"title":"Kombucha","description":"Fresh, organically grown Kombucha.","keywords":"Kombucha,Organic Beverages,organic,fresh","canonicalUrl":"/products/organic-beverages-kombucha"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Kombucha
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('6a8940b9-fd89-49de-bf33-ac2b5a7368b4','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('6a8940b9-fd89-49de-bf33-ac2b5a7368b4','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('6a8940b9-fd89-49de-bf33-ac2b5a7368b4','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('e7744376-cd93-4048-b2d9-ce7f21f82269','Vegetable juice','Fresh, organically grown Vegetable juice.',202,70,'7f5f0873-97ca-4a8e-a27e-a22e9c0a3eb8','FreshHarvest Market Organics','EP-ORG-VEGETA','bottle',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('70d8f838-be08-41c9-85af-31ba5042e248','e7744376-cd93-4048-b2d9-ce7f21f82269','organic-beverages-vegetable-juice','{"title":"Vegetable juice","description":"Fresh, organically grown Vegetable juice.","keywords":"Vegetable juice,Organic Beverages,organic,fresh","canonicalUrl":"/products/organic-beverages-vegetable-juice"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Vegetable juice
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e7744376-cd93-4048-b2d9-ce7f21f82269','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e7744376-cd93-4048-b2d9-ce7f21f82269','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e7744376-cd93-4048-b2d9-ce7f21f82269','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('5f44c07b-4c85-4f95-b0e6-b7886196730f','Cheddar cheese','Fresh, organically grown Cheddar cheese.',59,55,'9a5bbc16-ec30-4c87-b0d3-e9b4778e66a2','FreshHarvest Market Organics','EP-ORG-CHEDDA','pack',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('565a9dfb-d556-4eb9-b44e-ba6a92baabfb','5f44c07b-4c85-4f95-b0e6-b7886196730f','organic-dairy-products-cheddar-cheese','{"title":"Cheddar cheese","description":"Fresh, organically grown Cheddar cheese.","keywords":"Cheddar cheese,Organic Dairy Products,organic,fresh","canonicalUrl":"/products/organic-dairy-products-cheddar-cheese"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Cheddar cheese
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5f44c07b-4c85-4f95-b0e6-b7886196730f','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5f44c07b-4c85-4f95-b0e6-b7886196730f','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5f44c07b-4c85-4f95-b0e6-b7886196730f','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('9f237df5-721c-4a7c-aedf-f6fbfecd683e','Cottage cheese','Fresh, organically grown Cottage cheese.',82,91,'9a5bbc16-ec30-4c87-b0d3-e9b4778e66a2','FreshHarvest Market Organics','EP-ORG-COTTAG','pack',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('c7db0872-9596-471f-a5bb-c2ace8a88806','9f237df5-721c-4a7c-aedf-f6fbfecd683e','organic-dairy-products-cottage-cheese','{"title":"Cottage cheese","description":"Fresh, organically grown Cottage cheese.","keywords":"Cottage cheese,Organic Dairy Products,organic,fresh","canonicalUrl":"/products/organic-dairy-products-cottage-cheese"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Cottage cheese
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9f237df5-721c-4a7c-aedf-f6fbfecd683e','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9f237df5-721c-4a7c-aedf-f6fbfecd683e','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9f237df5-721c-4a7c-aedf-f6fbfecd683e','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('207ddb53-c370-490c-b240-de3d262790c0','Greek yogurt','Fresh, organically grown Greek yogurt.',133,18,'9a5bbc16-ec30-4c87-b0d3-e9b4778e66a2','FreshHarvest Market Organics','EP-ORG-GREEK-','pack',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('0f6c2807-6970-45f7-a329-d88eb2d161fc','207ddb53-c370-490c-b240-de3d262790c0','organic-dairy-products-greek-yogurt','{"title":"Greek yogurt","description":"Fresh, organically grown Greek yogurt.","keywords":"Greek yogurt,Organic Dairy Products,organic,fresh","canonicalUrl":"/products/organic-dairy-products-greek-yogurt"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Greek yogurt
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('207ddb53-c370-490c-b240-de3d262790c0','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('207ddb53-c370-490c-b240-de3d262790c0','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('207ddb53-c370-490c-b240-de3d262790c0','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('76468a49-bc53-4efa-9346-a8380df52db2','Kefir','Fresh, organically grown Kefir.',71,39,'9a5bbc16-ec30-4c87-b0d3-e9b4778e66a2','FreshHarvest Market Organics','EP-ORG-KEFIR','pack',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('65ce0546-ac0a-4d80-88d8-b73d5050dee6','76468a49-bc53-4efa-9346-a8380df52db2','organic-dairy-products-kefir','{"title":"Kefir","description":"Fresh, organically grown Kefir.","keywords":"Kefir,Organic Dairy Products,organic,fresh","canonicalUrl":"/products/organic-dairy-products-kefir"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Kefir
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('76468a49-bc53-4efa-9346-a8380df52db2','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('76468a49-bc53-4efa-9346-a8380df52db2','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('76468a49-bc53-4efa-9346-a8380df52db2','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('5546023b-f290-4115-a743-9532b45c2f71','Milk','Fresh, organically grown Milk.',132,91,'9a5bbc16-ec30-4c87-b0d3-e9b4778e66a2','FreshHarvest Market Organics','EP-ORG-MILK','pack',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('c6edeb63-6aa4-42ec-883f-417ccebac80c','5546023b-f290-4115-a743-9532b45c2f71','organic-dairy-products-milk','{"title":"Milk","description":"Fresh, organically grown Milk.","keywords":"Milk,Organic Dairy Products,organic,fresh","canonicalUrl":"/products/organic-dairy-products-milk"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Milk
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5546023b-f290-4115-a743-9532b45c2f71','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5546023b-f290-4115-a743-9532b45c2f71','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5546023b-f290-4115-a743-9532b45c2f71','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('7952c24f-925b-4624-b37a-af1d11b5cf9a','Apples','Fresh, organically grown Apples.',167,108,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-APPLES','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('5ee15579-b9ae-423a-9ef7-4f958a3fd299','7952c24f-925b-4624-b37a-af1d11b5cf9a','organic-fruits-apples','{"title":"Apples","description":"Fresh, organically grown Apples.","keywords":"Apples,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-apples"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Apples
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('7952c24f-925b-4624-b37a-af1d11b5cf9a','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('7952c24f-925b-4624-b37a-af1d11b5cf9a','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('7952c24f-925b-4624-b37a-af1d11b5cf9a','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('9cf942c1-97b1-4e12-b109-f58903d8d128','Avocados','Fresh, organically grown Avocados.',159,62,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-AVOCAD','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('2001e563-9323-4081-a033-555d49d8b6d8','9cf942c1-97b1-4e12-b109-f58903d8d128','organic-fruits-avocados','{"title":"Avocados","description":"Fresh, organically grown Avocados.","keywords":"Avocados,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-avocados"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Avocados
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9cf942c1-97b1-4e12-b109-f58903d8d128','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9cf942c1-97b1-4e12-b109-f58903d8d128','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9cf942c1-97b1-4e12-b109-f58903d8d128','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('40637382-edb6-4876-9d30-d954fa87a336','Bananas','Fresh, organically grown Bananas.',167,84,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-BANANA','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('7cad2e02-61f3-4928-a03a-fd011b2a838c','40637382-edb6-4876-9d30-d954fa87a336','organic-fruits-bananas','{"title":"Bananas","description":"Fresh, organically grown Bananas.","keywords":"Bananas,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-bananas"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Bananas
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('40637382-edb6-4876-9d30-d954fa87a336','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('40637382-edb6-4876-9d30-d954fa87a336','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('40637382-edb6-4876-9d30-d954fa87a336','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','Blueberries','Fresh, organically grown Blueberries.',116,88,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-BLUEBE','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('53ad5a85-7c1e-4a86-8b6e-24835c9588ec','5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','organic-fruits-blueberries','{"title":"Blueberries","description":"Fresh, organically grown Blueberries.","keywords":"Blueberries,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-blueberries"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Blueberries
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('4c8f7cef-9baf-48c3-aa93-66d7c692d253','Grapes','Fresh, organically grown Grapes.',124,21,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-GRAPES','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('9b877744-ff5e-446c-a42f-fcf1186893f0','4c8f7cef-9baf-48c3-aa93-66d7c692d253','organic-fruits-grapes','{"title":"Grapes","description":"Fresh, organically grown Grapes.","keywords":"Grapes,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-grapes"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Grapes
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4c8f7cef-9baf-48c3-aa93-66d7c692d253','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4c8f7cef-9baf-48c3-aa93-66d7c692d253','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4c8f7cef-9baf-48c3-aa93-66d7c692d253','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','Mangoes','Fresh, organically grown Mangoes.',219,79,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-MANGOE','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('f9923553-8d69-4099-be8e-43209e398f30','e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','organic-fruits-mangoes','{"title":"Mangoes","description":"Fresh, organically grown Mangoes.","keywords":"Mangoes,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-mangoes"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Mangoes
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('e09384b6-2e54-45e2-8224-b4857d229f1d','Oranges','Fresh, organically grown Oranges.',114,51,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-ORANGE','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('1824a95b-0a8b-4a61-9322-4c3f9ad5ec35','e09384b6-2e54-45e2-8224-b4857d229f1d','organic-fruits-oranges','{"title":"Oranges","description":"Fresh, organically grown Oranges.","keywords":"Oranges,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-oranges"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Oranges
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e09384b6-2e54-45e2-8224-b4857d229f1d','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e09384b6-2e54-45e2-8224-b4857d229f1d','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e09384b6-2e54-45e2-8224-b4857d229f1d','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('1ebaf900-261c-4d6a-89be-29e4cd8b37c0','Pomegranates','Fresh, organically grown Pomegranates.',195,25,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-POMEGR','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('241cd88c-d7ef-4874-9b42-fd2107bebdd1','1ebaf900-261c-4d6a-89be-29e4cd8b37c0','organic-fruits-pomegranates','{"title":"Pomegranates","description":"Fresh, organically grown Pomegranates.","keywords":"Pomegranates,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-pomegranates"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Pomegranates
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('1ebaf900-261c-4d6a-89be-29e4cd8b37c0','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('1ebaf900-261c-4d6a-89be-29e4cd8b37c0','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('1ebaf900-261c-4d6a-89be-29e4cd8b37c0','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','Raspberries','Fresh, organically grown Raspberries.',163,98,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-RASPBE','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('892303c9-ff07-4cf3-90f5-95d2ae5f5480','b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','organic-fruits-raspberries','{"title":"Raspberries","description":"Fresh, organically grown Raspberries.","keywords":"Raspberries,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-raspberries"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Raspberries
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('81fa9fab-55ee-4f33-97a5-8370f785a1e2','Strawberries','Fresh, organically grown Strawberries.',176,72,'3c5f7f01-c2f2-4142-8532-da208cfa29de','FreshHarvest Market Organics','EP-ORG-STRAWB','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('e884c9e1-d3fa-473b-871a-5cdcd1a871b1','81fa9fab-55ee-4f33-97a5-8370f785a1e2','organic-fruits-strawberries','{"title":"Strawberries","description":"Fresh, organically grown Strawberries.","keywords":"Strawberries,Organic Fruits,organic,fresh","canonicalUrl":"/products/organic-fruits-strawberries"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Strawberries
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('81fa9fab-55ee-4f33-97a5-8370f785a1e2','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('81fa9fab-55ee-4f33-97a5-8370f785a1e2','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('81fa9fab-55ee-4f33-97a5-8370f785a1e2','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('c2b7565e-d21d-4a62-814c-8154eb091b6d','Brown rice','Fresh, organically grown Brown rice.',64,111,'a33a5368-518b-4ed2-895e-0c71a5a81670','FreshHarvest Market Organics','EP-ORG-BROWN-','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('4600006e-5cac-438a-904c-9819cf3e8ab5','c2b7565e-d21d-4a62-814c-8154eb091b6d','organic-grains-cereals-brown-rice','{"title":"Brown rice","description":"Fresh, organically grown Brown rice.","keywords":"Brown rice,Organic Grains & Cereals,organic,fresh","canonicalUrl":"/products/organic-grains-cereals-brown-rice"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Brown rice
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('c2b7565e-d21d-4a62-814c-8154eb091b6d','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('c2b7565e-d21d-4a62-814c-8154eb091b6d','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('c2b7565e-d21d-4a62-814c-8154eb091b6d','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('66b331ee-6099-40c4-866e-97e2092d5076','Buckwheat','Fresh, organically grown Buckwheat.',125,66,'a33a5368-518b-4ed2-895e-0c71a5a81670','FreshHarvest Market Organics','EP-ORG-BUCKWH','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('3813d3ae-7ff7-4e17-9723-e7cc534feefc','66b331ee-6099-40c4-866e-97e2092d5076','organic-grains-cereals-buckwheat','{"title":"Buckwheat","description":"Fresh, organically grown Buckwheat.","keywords":"Buckwheat,Organic Grains & Cereals,organic,fresh","canonicalUrl":"/products/organic-grains-cereals-buckwheat"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Buckwheat
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('66b331ee-6099-40c4-866e-97e2092d5076','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('66b331ee-6099-40c4-866e-97e2092d5076','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('66b331ee-6099-40c4-866e-97e2092d5076','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('a114955e-0c49-4b92-8950-f0dcee779bca','Millet','Fresh, organically grown Millet.',69,97,'a33a5368-518b-4ed2-895e-0c71a5a81670','FreshHarvest Market Organics','EP-ORG-MILLET','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('da878af9-e6ac-4274-9269-549a9566c0c7','a114955e-0c49-4b92-8950-f0dcee779bca','organic-grains-cereals-millet','{"title":"Millet","description":"Fresh, organically grown Millet.","keywords":"Millet,Organic Grains & Cereals,organic,fresh","canonicalUrl":"/products/organic-grains-cereals-millet"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Millet
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a114955e-0c49-4b92-8950-f0dcee779bca','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a114955e-0c49-4b92-8950-f0dcee779bca','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a114955e-0c49-4b92-8950-f0dcee779bca','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','Oats','Fresh, organically grown Oats.',66,98,'a33a5368-518b-4ed2-895e-0c71a5a81670','FreshHarvest Market Organics','EP-ORG-OATS','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('b938273e-fe99-401f-8933-059e45e6a089','4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','organic-grains-cereals-oats','{"title":"Oats","description":"Fresh, organically grown Oats.","keywords":"Oats,Organic Grains & Cereals,organic,fresh","canonicalUrl":"/products/organic-grains-cereals-oats"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Oats
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('3404a775-b1b9-42fa-a7db-dc156d966dbb','Quinoa','Fresh, organically grown Quinoa.',55,22,'a33a5368-518b-4ed2-895e-0c71a5a81670','FreshHarvest Market Organics','EP-ORG-QUINOA','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('f6089c18-c26b-4380-9400-763bed7fd1f8','3404a775-b1b9-42fa-a7db-dc156d966dbb','organic-grains-cereals-quinoa','{"title":"Quinoa","description":"Fresh, organically grown Quinoa.","keywords":"Quinoa,Organic Grains & Cereals,organic,fresh","canonicalUrl":"/products/organic-grains-cereals-quinoa"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Quinoa
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('3404a775-b1b9-42fa-a7db-dc156d966dbb','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('3404a775-b1b9-42fa-a7db-dc156d966dbb','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('3404a775-b1b9-42fa-a7db-dc156d966dbb','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('2fe16402-0147-4b6b-945b-e40baaf77147','Basil','Fresh, organically grown Basil.',53,23,'14b7e158-ae37-4441-8868-97d15ff787d4','FreshHarvest Market Organics','EP-ORG-BASIL','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('2146bc41-aadf-4e71-bbb3-68f0f3987129','2fe16402-0147-4b6b-945b-e40baaf77147','organic-herbs-spices-basil','{"title":"Basil","description":"Fresh, organically grown Basil.","keywords":"Basil,Organic Herbs & Spices,organic,fresh","canonicalUrl":"/products/organic-herbs-spices-basil"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Basil
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('2fe16402-0147-4b6b-945b-e40baaf77147','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('2fe16402-0147-4b6b-945b-e40baaf77147','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('2fe16402-0147-4b6b-945b-e40baaf77147','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','Cinnamon','Fresh, organically grown Cinnamon.',73,62,'14b7e158-ae37-4441-8868-97d15ff787d4','FreshHarvest Market Organics','EP-ORG-CINNAM','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('d5f3d51c-9c22-4d53-9d22-b502ea894096','ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','organic-herbs-spices-cinnamon','{"title":"Cinnamon","description":"Fresh, organically grown Cinnamon.","keywords":"Cinnamon,Organic Herbs & Spices,organic,fresh","canonicalUrl":"/products/organic-herbs-spices-cinnamon"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Cinnamon
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('352a4f89-13e3-4fa2-9010-070a9ee70562','Ginger','Fresh, organically grown Ginger.',74,112,'14b7e158-ae37-4441-8868-97d15ff787d4','FreshHarvest Market Organics','EP-ORG-GINGER','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('26d91a2f-401a-40d3-8d42-7227ce0c0eb5','352a4f89-13e3-4fa2-9010-070a9ee70562','organic-herbs-spices-ginger','{"title":"Ginger","description":"Fresh, organically grown Ginger.","keywords":"Ginger,Organic Herbs & Spices,organic,fresh","canonicalUrl":"/products/organic-herbs-spices-ginger"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Ginger
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('352a4f89-13e3-4fa2-9010-070a9ee70562','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('352a4f89-13e3-4fa2-9010-070a9ee70562','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('352a4f89-13e3-4fa2-9010-070a9ee70562','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('006f2cc1-54f8-4ed2-9d6f-ac454592ea59','Oregano','Fresh, organically grown Oregano.',61,96,'14b7e158-ae37-4441-8868-97d15ff787d4','FreshHarvest Market Organics','EP-ORG-OREGAN','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('891eab8e-f33c-4cd8-8a4d-a0c12fd75b2e','006f2cc1-54f8-4ed2-9d6f-ac454592ea59','organic-herbs-spices-oregano','{"title":"Oregano","description":"Fresh, organically grown Oregano.","keywords":"Oregano,Organic Herbs & Spices,organic,fresh","canonicalUrl":"/products/organic-herbs-spices-oregano"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Oregano
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('006f2cc1-54f8-4ed2-9d6f-ac454592ea59','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('006f2cc1-54f8-4ed2-9d6f-ac454592ea59','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('006f2cc1-54f8-4ed2-9d6f-ac454592ea59','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('33241df6-2896-4045-8d24-9c0e26b7304f','Turmeric','Fresh, organically grown Turmeric.',78,17,'14b7e158-ae37-4441-8868-97d15ff787d4','FreshHarvest Market Organics','EP-ORG-TURMER','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('f4f3a741-6a74-490c-b9c1-a97bb3c32b14','33241df6-2896-4045-8d24-9c0e26b7304f','organic-herbs-spices-turmeric','{"title":"Turmeric","description":"Fresh, organically grown Turmeric.","keywords":"Turmeric,Organic Herbs & Spices,organic,fresh","canonicalUrl":"/products/organic-herbs-spices-turmeric"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Turmeric
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('33241df6-2896-4045-8d24-9c0e26b7304f','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('33241df6-2896-4045-8d24-9c0e26b7304f','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('33241df6-2896-4045-8d24-9c0e26b7304f','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('4ef271d3-d973-43e4-a23e-6d16fd944987','Black beans','Fresh, organically grown Black beans.',117,19,'75e9f924-74bc-4b9f-9271-71ff69a6dab1','FreshHarvest Market Organics','EP-ORG-BLACK-','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('287bf484-ab71-4f4f-b336-61da09bb746b','4ef271d3-d973-43e4-a23e-6d16fd944987','organic-legumes-pulses-black-beans','{"title":"Black beans","description":"Fresh, organically grown Black beans.","keywords":"Black beans,Organic Legumes & Pulses,organic,fresh","canonicalUrl":"/products/organic-legumes-pulses-black-beans"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Black beans
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4ef271d3-d973-43e4-a23e-6d16fd944987','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4ef271d3-d973-43e4-a23e-6d16fd944987','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('4ef271d3-d973-43e4-a23e-6d16fd944987','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('a430db4f-8964-4a0f-bcce-7a7922e6b013','Chickpeas','Fresh, organically grown Chickpeas.',112,16,'75e9f924-74bc-4b9f-9271-71ff69a6dab1','FreshHarvest Market Organics','EP-ORG-CHICKP','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('b9568bba-9270-42fc-a0ad-7124f3f8beb2','a430db4f-8964-4a0f-bcce-7a7922e6b013','organic-legumes-pulses-chickpeas','{"title":"Chickpeas","description":"Fresh, organically grown Chickpeas.","keywords":"Chickpeas,Organic Legumes & Pulses,organic,fresh","canonicalUrl":"/products/organic-legumes-pulses-chickpeas"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Chickpeas
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a430db4f-8964-4a0f-bcce-7a7922e6b013','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a430db4f-8964-4a0f-bcce-7a7922e6b013','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a430db4f-8964-4a0f-bcce-7a7922e6b013','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('405bfb55-db4f-4aa3-ad7c-ff507b98f526','Green peas','Fresh, organically grown Green peas.',155,88,'75e9f924-74bc-4b9f-9271-71ff69a6dab1','FreshHarvest Market Organics','EP-ORG-GREEN-','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('5cbdd816-48fb-4dc1-ba12-1945746bbfd9','405bfb55-db4f-4aa3-ad7c-ff507b98f526','organic-legumes-pulses-green-peas','{"title":"Green peas","description":"Fresh, organically grown Green peas.","keywords":"Green peas,Organic Legumes & Pulses,organic,fresh","canonicalUrl":"/products/organic-legumes-pulses-green-peas"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Green peas
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('405bfb55-db4f-4aa3-ad7c-ff507b98f526','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('405bfb55-db4f-4aa3-ad7c-ff507b98f526','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('405bfb55-db4f-4aa3-ad7c-ff507b98f526','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('b8817db1-18b6-404c-bb42-17d1e87afb36','Kidney beans','Fresh, organically grown Kidney beans.',125,43,'75e9f924-74bc-4b9f-9271-71ff69a6dab1','FreshHarvest Market Organics','EP-ORG-KIDNEY','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('62e62f08-f457-41bf-94b2-20dc58fb453f','b8817db1-18b6-404c-bb42-17d1e87afb36','organic-legumes-pulses-kidney-beans','{"title":"Kidney beans","description":"Fresh, organically grown Kidney beans.","keywords":"Kidney beans,Organic Legumes & Pulses,organic,fresh","canonicalUrl":"/products/organic-legumes-pulses-kidney-beans"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Kidney beans
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b8817db1-18b6-404c-bb42-17d1e87afb36','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b8817db1-18b6-404c-bb42-17d1e87afb36','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b8817db1-18b6-404c-bb42-17d1e87afb36','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','Lentils','Fresh, organically grown Lentils.',104,68,'75e9f924-74bc-4b9f-9271-71ff69a6dab1','FreshHarvest Market Organics','EP-ORG-LENTIL','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('17c2256e-ff3f-47fe-9f4f-68a123dd4a0b','0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','organic-legumes-pulses-lentils','{"title":"Lentils","description":"Fresh, organically grown Lentils.","keywords":"Lentils,Organic Legumes & Pulses,organic,fresh","canonicalUrl":"/products/organic-legumes-pulses-lentils"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Lentils
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('eda76370-d2c8-4a74-a602-910f443a2b95','Almonds','Fresh, organically grown Almonds.',363,37,'b410d77f-f06c-4223-8c8e-d626f65fdd20','FreshHarvest Market Organics','EP-ORG-ALMOND','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('5f30ee34-39d4-46be-b287-55ec2a5bd410','eda76370-d2c8-4a74-a602-910f443a2b95','organic-nuts-seeds-almonds','{"title":"Almonds","description":"Fresh, organically grown Almonds.","keywords":"Almonds,Organic Nuts & Seeds,organic,fresh","canonicalUrl":"/products/organic-nuts-seeds-almonds"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Almonds
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('eda76370-d2c8-4a74-a602-910f443a2b95','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('eda76370-d2c8-4a74-a602-910f443a2b95','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('eda76370-d2c8-4a74-a602-910f443a2b95','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('e414a976-8ab2-4da4-ba50-b34927ae0d65','Chia seeds','Fresh, organically grown Chia seeds.',581,15,'b410d77f-f06c-4223-8c8e-d626f65fdd20','FreshHarvest Market Organics','EP-ORG-CHIA-S','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('545f393d-084b-47ae-8492-1d3f7aad6d8b','e414a976-8ab2-4da4-ba50-b34927ae0d65','organic-nuts-seeds-chia-seeds','{"title":"Chia seeds","description":"Fresh, organically grown Chia seeds.","keywords":"Chia seeds,Organic Nuts & Seeds,organic,fresh","canonicalUrl":"/products/organic-nuts-seeds-chia-seeds"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Chia seeds
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e414a976-8ab2-4da4-ba50-b34927ae0d65','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e414a976-8ab2-4da4-ba50-b34927ae0d65','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('e414a976-8ab2-4da4-ba50-b34927ae0d65','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('b5538745-c0b2-446c-b3d9-345d1e9c663b','Flaxseeds','Fresh, organically grown Flaxseeds.',846,40,'b410d77f-f06c-4223-8c8e-d626f65fdd20','FreshHarvest Market Organics','EP-ORG-FLAXSE','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('3022d5cf-64e9-4032-9cd9-7516eebcccdc','b5538745-c0b2-446c-b3d9-345d1e9c663b','organic-nuts-seeds-flaxseeds','{"title":"Flaxseeds","description":"Fresh, organically grown Flaxseeds.","keywords":"Flaxseeds,Organic Nuts & Seeds,organic,fresh","canonicalUrl":"/products/organic-nuts-seeds-flaxseeds"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Flaxseeds
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b5538745-c0b2-446c-b3d9-345d1e9c663b','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b5538745-c0b2-446c-b3d9-345d1e9c663b','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('b5538745-c0b2-446c-b3d9-345d1e9c663b','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','Pumpkin seeds','Fresh, organically grown Pumpkin seeds.',919,17,'b410d77f-f06c-4223-8c8e-d626f65fdd20','FreshHarvest Market Organics','EP-ORG-PUMPKI','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('42da1d58-8773-432f-891b-206238c768a2','9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','organic-nuts-seeds-pumpkin-seeds','{"title":"Pumpkin seeds","description":"Fresh, organically grown Pumpkin seeds.","keywords":"Pumpkin seeds,Organic Nuts & Seeds,organic,fresh","canonicalUrl":"/products/organic-nuts-seeds-pumpkin-seeds"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Pumpkin seeds
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('53a9112f-a7ac-44bf-a007-c968b617ea27','Walnuts','Fresh, organically grown Walnuts.',329,12,'b410d77f-f06c-4223-8c8e-d626f65fdd20','FreshHarvest Market Organics','EP-ORG-WALNUT','kg',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('809e62e5-58b7-42cd-8763-66bd0b9c0600','53a9112f-a7ac-44bf-a007-c968b617ea27','organic-nuts-seeds-walnuts','{"title":"Walnuts","description":"Fresh, organically grown Walnuts.","keywords":"Walnuts,Organic Nuts & Seeds,organic,fresh","canonicalUrl":"/products/organic-nuts-seeds-walnuts"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Walnuts
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('53a9112f-a7ac-44bf-a007-c968b617ea27','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('53a9112f-a7ac-44bf-a007-c968b617ea27','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('53a9112f-a7ac-44bf-a007-c968b617ea27','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('5a90ff57-d854-4da2-9351-979a0f908b10','Beets','Fresh, organically grown Beets.',142,108,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-BEETS','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('37fed772-c5a9-4d13-9dd2-ad789b909e0a','5a90ff57-d854-4da2-9351-979a0f908b10','organic-vegetables-beets','{"title":"Beets","description":"Fresh, organically grown Beets.","keywords":"Beets,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-beets"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Beets
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5a90ff57-d854-4da2-9351-979a0f908b10','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5a90ff57-d854-4da2-9351-979a0f908b10','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('5a90ff57-d854-4da2-9351-979a0f908b10','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','Bell peppers','Fresh, organically grown Bell peppers.',76,50,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-BELL-P','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('beeada70-b9b8-4b00-a120-dc10de9f3394','dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','organic-vegetables-bell-peppers','{"title":"Bell peppers","description":"Fresh, organically grown Bell peppers.","keywords":"Bell peppers,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-bell-peppers"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Bell peppers
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('13cb79e0-ff9c-40bf-b4c4-178faf796b38','Broccoli','Fresh, organically grown Broccoli.',89,94,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-BROCCO','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('2e166c51-1821-4741-a747-6bcab878cddd','13cb79e0-ff9c-40bf-b4c4-178faf796b38','organic-vegetables-broccoli','{"title":"Broccoli","description":"Fresh, organically grown Broccoli.","keywords":"Broccoli,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-broccoli"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Broccoli
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('13cb79e0-ff9c-40bf-b4c4-178faf796b38','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('13cb79e0-ff9c-40bf-b4c4-178faf796b38','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('13cb79e0-ff9c-40bf-b4c4-178faf796b38','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('d89c0187-8b38-4efb-bcd2-44188a15cb5d','Brussels sprouts','Fresh, organically grown Brussels sprouts.',147,91,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-BRUSSE','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('997b61fe-8905-4e20-b2d1-a30ba1f5e57e','d89c0187-8b38-4efb-bcd2-44188a15cb5d','organic-vegetables-brussels-sprouts','{"title":"Brussels sprouts","description":"Fresh, organically grown Brussels sprouts.","keywords":"Brussels sprouts,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-brussels-sprouts"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Brussels sprouts
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('d89c0187-8b38-4efb-bcd2-44188a15cb5d','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('d89c0187-8b38-4efb-bcd2-44188a15cb5d','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('d89c0187-8b38-4efb-bcd2-44188a15cb5d','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('59873b7f-712c-42f0-a7bd-67f1568c0a92','Carrots','Fresh, organically grown Carrots.',68,72,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-CARROT','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('c409ea82-b928-4dc0-842a-b2aa3a575c4b','59873b7f-712c-42f0-a7bd-67f1568c0a92','organic-vegetables-carrots','{"title":"Carrots","description":"Fresh, organically grown Carrots.","keywords":"Carrots,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-carrots"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Carrots
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('59873b7f-712c-42f0-a7bd-67f1568c0a92','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('59873b7f-712c-42f0-a7bd-67f1568c0a92','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('59873b7f-712c-42f0-a7bd-67f1568c0a92','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('da6d6abb-138d-45a1-9ec5-6cb5f997f297','Cauliflower','Fresh, organically grown Cauliflower.',116,39,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-CAULIF','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('33a58f1e-4d83-4091-a03f-6463e426132e','da6d6abb-138d-45a1-9ec5-6cb5f997f297','organic-vegetables-cauliflower','{"title":"Cauliflower","description":"Fresh, organically grown Cauliflower.","keywords":"Cauliflower,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-cauliflower"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Cauliflower
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('da6d6abb-138d-45a1-9ec5-6cb5f997f297','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('da6d6abb-138d-45a1-9ec5-6cb5f997f297','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('da6d6abb-138d-45a1-9ec5-6cb5f997f297','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','Kale','Fresh, organically grown Kale.',153,98,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-KALE','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('a37958a2-e9e6-42bc-86c5-839dd1f7275c','19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','organic-vegetables-kale','{"title":"Kale","description":"Fresh, organically grown Kale.","keywords":"Kale,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-kale"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Kale
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('ab8d6bc4-68af-4103-a778-428788914fa2','Spinach','Fresh, organically grown Spinach.',121,74,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-SPINAC','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('f00a0eba-e5d0-4d0c-85eb-fc32fecd6351','ab8d6bc4-68af-4103-a778-428788914fa2','organic-vegetables-spinach','{"title":"Spinach","description":"Fresh, organically grown Spinach.","keywords":"Spinach,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-spinach"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Spinach
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('ab8d6bc4-68af-4103-a778-428788914fa2','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('ab8d6bc4-68af-4103-a778-428788914fa2','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('ab8d6bc4-68af-4103-a778-428788914fa2','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('a274943b-6f07-450f-9b3d-c9a79d80b232','Sweet potato','Fresh, organically grown Sweet potato.',132,104,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-SWEET-','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('8d65036f-5b34-46b1-8296-88c66cd016aa','a274943b-6f07-450f-9b3d-c9a79d80b232','organic-vegetables-sweet-potato','{"title":"Sweet potato","description":"Fresh, organically grown Sweet potato.","keywords":"Sweet potato,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-sweet-potato"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Sweet potato
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a274943b-6f07-450f-9b3d-c9a79d80b232','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a274943b-6f07-450f-9b3d-c9a79d80b232','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('a274943b-6f07-450f-9b3d-c9a79d80b232','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])
  VALUES ('93b85e86-bd44-46b2-90dc-2872cdb9f071','Tomatoes','Fresh, organically grown Tomatoes.',93,32,'954671e4-0b9f-477b-8ee8-e2f0ba87138a','FreshHarvest Market Organics','EP-ORG-TOMATO','bunch',1,SYSUTCDATETIME(),NULL);

  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])
  VALUES ('83b57531-fdfa-4b05-a221-1261c16f59d9','93b85e86-bd44-46b2-90dc-2872cdb9f071','organic-vegetables-tomatoes','{"title":"Tomatoes","description":"Fresh, organically grown Tomatoes.","keywords":"Tomatoes,Organic Vegetables,organic,fresh","canonicalUrl":"/products/organic-vegetables-tomatoes"}',SYSUTCDATETIME(),NULL);

  -- Default tags for Tomatoes
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('93b85e86-bd44-46b2-90dc-2872cdb9f071','8fd11ba1-58ed-4c2e-9d9f-08d6ef11b3a0');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('93b85e86-bd44-46b2-90dc-2872cdb9f071','de7ce5a0-7d59-4979-bcc9-ed78bdfca273');
  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('93b85e86-bd44-46b2-90dc-2872cdb9f071','bf80a8f8-280a-4b3c-ad79-32548705e17c');

  -- Product Images
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9ab41bae-72a1-46c1-83dc-e2f5701aee16','4947208a-334e-4373-ba27-cbcab282bc6c','/product-images/organic-beverages/chamomile-tea/1.png','Chamomile tea image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('025b43bb-0e85-415f-8775-1f5865a8190a','4947208a-334e-4373-ba27-cbcab282bc6c','/product-images/organic-beverages/chamomile-tea/3.jpg','Chamomile tea image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c36deb28-da4a-4445-9506-62dfbf05708c','4947208a-334e-4373-ba27-cbcab282bc6c','/product-images/organic-beverages/chamomile-tea/4.jpg','Chamomile tea image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('14f63a3d-d38f-4243-83b2-a684c0ae3f05','4947208a-334e-4373-ba27-cbcab282bc6c','/product-images/organic-beverages/chamomile-tea/5.jpg','Chamomile tea image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e9293cd8-d3cd-444f-864a-89381b8e1d74','22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','/product-images/organic-beverages/coconut-water/1.jpg','Coconut water image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2804bbf1-39c2-4866-af61-6f433b6a137a','22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','/product-images/organic-beverages/coconut-water/2.jpg','Coconut water image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('71ff810e-eabe-4e3b-bd75-086ddaca6b9c','22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','/product-images/organic-beverages/coconut-water/3.jpg','Coconut water image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7ed8d657-7a77-4b21-81ff-8a96aca840aa','22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','/product-images/organic-beverages/coconut-water/4.jpg','Coconut water image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9734f98e-c4cf-40af-9977-732d67065728','22b92d6e-880e-43d4-8bb5-dcc88f8a6b75','/product-images/organic-beverages/coconut-water/5.jpg','Coconut water image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('86608f03-58d8-4c12-83c2-7005936efc4f','8f497f77-77b9-47c5-954d-a8bac6893d4b','/product-images/organic-beverages/green-tea/1.jpg','Green tea image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('63382f05-619c-4eaf-9ff2-01da16a4d243','8f497f77-77b9-47c5-954d-a8bac6893d4b','/product-images/organic-beverages/green-tea/2.jpg','Green tea image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fcf294c1-ac29-4abc-8d56-730877fc9c2a','8f497f77-77b9-47c5-954d-a8bac6893d4b','/product-images/organic-beverages/green-tea/3.jpg','Green tea image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('b7f600e2-483e-422f-9fff-ac59bd648d17','8f497f77-77b9-47c5-954d-a8bac6893d4b','/product-images/organic-beverages/green-tea/4.jpg','Green tea image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('90b8fc8a-ef05-454d-966d-44df63af22d2','8f497f77-77b9-47c5-954d-a8bac6893d4b','/product-images/organic-beverages/green-tea/5.jpg','Green tea image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a20bc7f0-7b8d-4cc4-a49d-307c5ca0ca8e','6a8940b9-fd89-49de-bf33-ac2b5a7368b4','/product-images/organic-beverages/kombucha/1.png','Kombucha image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('eadc1e1f-9282-4028-8113-a3cef7c819da','6a8940b9-fd89-49de-bf33-ac2b5a7368b4','/product-images/organic-beverages/kombucha/2.jpg','Kombucha image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fe19225a-f52a-4d68-a814-d0c791d9984b','6a8940b9-fd89-49de-bf33-ac2b5a7368b4','/product-images/organic-beverages/kombucha/3.jpg','Kombucha image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9ad59d36-e3dd-40fe-b98c-4c1ce3049124','6a8940b9-fd89-49de-bf33-ac2b5a7368b4','/product-images/organic-beverages/kombucha/4.png','Kombucha image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8c5880cc-cf88-4db5-a0ff-72e06a4b7812','6a8940b9-fd89-49de-bf33-ac2b5a7368b4','/product-images/organic-beverages/kombucha/5.jpg','Kombucha image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2d84b61b-a58a-4cfa-b535-ea8647ca9ba6','e7744376-cd93-4048-b2d9-ce7f21f82269','/product-images/organic-beverages/vegetable-juice/2.jpg','Vegetable juice image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c09d0258-d604-4f5d-8fa7-d30f6e9e9ffd','e7744376-cd93-4048-b2d9-ce7f21f82269','/product-images/organic-beverages/vegetable-juice/3.jpg','Vegetable juice image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('cb79889c-4cdf-47ba-9e6c-01521cd1564d','e7744376-cd93-4048-b2d9-ce7f21f82269','/product-images/organic-beverages/vegetable-juice/4.jpg','Vegetable juice image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('034cfb04-3d14-4642-8904-96508ded1e82','e7744376-cd93-4048-b2d9-ce7f21f82269','/product-images/organic-beverages/vegetable-juice/5.jpg','Vegetable juice image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c80e0a2e-573f-4616-9d2e-5a1f453f0e1b','5f44c07b-4c85-4f95-b0e6-b7886196730f','/product-images/organic-dairy-products/cheddar-cheese/1.jpg','Cheddar cheese image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7cd65372-2b25-4d51-9e04-7c3f9dc0d3c3','5f44c07b-4c85-4f95-b0e6-b7886196730f','/product-images/organic-dairy-products/cheddar-cheese/2.jpg','Cheddar cheese image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('63653550-04a1-463f-8a46-d373eecc7ecf','5f44c07b-4c85-4f95-b0e6-b7886196730f','/product-images/organic-dairy-products/cheddar-cheese/3.jpg','Cheddar cheese image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9161765d-1e3a-4b1f-98de-be8a685c8694','5f44c07b-4c85-4f95-b0e6-b7886196730f','/product-images/organic-dairy-products/cheddar-cheese/4.jpg','Cheddar cheese image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3fa4412f-9214-4ac2-9996-f18e75ac3cc5','5f44c07b-4c85-4f95-b0e6-b7886196730f','/product-images/organic-dairy-products/cheddar-cheese/5.jpg','Cheddar cheese image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a2b6635f-c653-40d2-af5d-4881f8b51522','9f237df5-721c-4a7c-aedf-f6fbfecd683e','/product-images/organic-dairy-products/cottage-cheese/1.jpg','Cottage cheese image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fc922d1f-142c-4a3a-aae5-42078bb91412','9f237df5-721c-4a7c-aedf-f6fbfecd683e','/product-images/organic-dairy-products/cottage-cheese/2.jpg','Cottage cheese image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('bb2a1b1d-29f6-4b11-a533-43f35caad420','9f237df5-721c-4a7c-aedf-f6fbfecd683e','/product-images/organic-dairy-products/cottage-cheese/3.jpg','Cottage cheese image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('15a60878-76d0-4603-a621-4c1926a7ff8d','9f237df5-721c-4a7c-aedf-f6fbfecd683e','/product-images/organic-dairy-products/cottage-cheese/4.jpg','Cottage cheese image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('220bd312-0557-4e69-a837-af351a7bf5e8','9f237df5-721c-4a7c-aedf-f6fbfecd683e','/product-images/organic-dairy-products/cottage-cheese/5.jpg','Cottage cheese image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f32f3916-f3b1-474c-a813-c6f6e934a917','207ddb53-c370-490c-b240-de3d262790c0','/product-images/organic-dairy-products/greek-yogurt/1.jpg','Greek yogurt image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('999e1d66-09a8-4122-a2ad-53453b622917','207ddb53-c370-490c-b240-de3d262790c0','/product-images/organic-dairy-products/greek-yogurt/2.jpg','Greek yogurt image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('109f76bf-91c3-42cd-9ae4-9a1fd2667500','207ddb53-c370-490c-b240-de3d262790c0','/product-images/organic-dairy-products/greek-yogurt/3.jpg','Greek yogurt image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('4c2c44e3-cd9a-470d-806e-895322071226','207ddb53-c370-490c-b240-de3d262790c0','/product-images/organic-dairy-products/greek-yogurt/4.jpg','Greek yogurt image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f3113816-59a4-4178-af50-25ec420eef1b','207ddb53-c370-490c-b240-de3d262790c0','/product-images/organic-dairy-products/greek-yogurt/5.jpg','Greek yogurt image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7516f100-7fc3-4ade-a329-1bbcdfdc7095','76468a49-bc53-4efa-9346-a8380df52db2','/product-images/organic-dairy-products/kefir/1.jpg','Kefir image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('dacef726-05bc-483e-a5a1-a791a731f902','76468a49-bc53-4efa-9346-a8380df52db2','/product-images/organic-dairy-products/kefir/2.jpg','Kefir image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('4072c40d-557a-476a-af7b-3adf4d80e9dc','76468a49-bc53-4efa-9346-a8380df52db2','/product-images/organic-dairy-products/kefir/3.jpg','Kefir image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('0e626c08-44aa-445e-ab0b-ec1d6dd2ebe2','76468a49-bc53-4efa-9346-a8380df52db2','/product-images/organic-dairy-products/kefir/4.jpg','Kefir image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e9085533-30fe-468c-abe2-d0abd97eaf27','76468a49-bc53-4efa-9346-a8380df52db2','/product-images/organic-dairy-products/kefir/5.jpg','Kefir image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('04025827-83af-4172-ba43-caadf9183fdc','5546023b-f290-4115-a743-9532b45c2f71','/product-images/organic-dairy-products/milk/1.jpg','Milk image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3fa85af3-d345-4205-aedf-8fc02efe59cd','5546023b-f290-4115-a743-9532b45c2f71','/product-images/organic-dairy-products/milk/2.jpg','Milk image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7e3a8f2b-383f-425a-b2e1-ea1b1da5bae6','5546023b-f290-4115-a743-9532b45c2f71','/product-images/organic-dairy-products/milk/4.jpg','Milk image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d69b2946-5f3d-4271-86ff-fef9ea4970b8','5546023b-f290-4115-a743-9532b45c2f71','/product-images/organic-dairy-products/milk/5.jpg','Milk image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('65118466-4dbf-4480-92c3-0c3aa9e3f1be','7952c24f-925b-4624-b37a-af1d11b5cf9a','/product-images/organic-fruits/apples/3.jpg','Apples image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3725aef2-a986-486d-a156-76785d008494','7952c24f-925b-4624-b37a-af1d11b5cf9a','/product-images/organic-fruits/apples/4.jpg','Apples image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a8f4c59d-4991-4720-9c4a-e72446c614ec','7952c24f-925b-4624-b37a-af1d11b5cf9a','/product-images/organic-fruits/apples/5.jpg','Apples image',2,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('083bcde8-f61f-4141-817c-9acdb35c08b6','9cf942c1-97b1-4e12-b109-f58903d8d128','/product-images/organic-fruits/avocados/1.jpg','Avocados image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('65e6ad36-8f4e-42ac-ab9f-c95bd8016613','9cf942c1-97b1-4e12-b109-f58903d8d128','/product-images/organic-fruits/avocados/2.jpg','Avocados image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('98c7a49a-6ad4-4a0a-9926-0e30d0eff4d2','9cf942c1-97b1-4e12-b109-f58903d8d128','/product-images/organic-fruits/avocados/3.jpg','Avocados image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('bed4dbb2-9dc7-4239-bc87-c44db8d9df92','9cf942c1-97b1-4e12-b109-f58903d8d128','/product-images/organic-fruits/avocados/4.jpg','Avocados image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f8c150ed-1e2b-4156-b7a1-a7e6019290b0','9cf942c1-97b1-4e12-b109-f58903d8d128','/product-images/organic-fruits/avocados/5.jpg','Avocados image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7dd0e790-971b-4a10-8161-5949377d69b7','40637382-edb6-4876-9d30-d954fa87a336','/product-images/organic-fruits/bananas/1.jpg','Bananas image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('73d71f12-e3af-40f8-a349-18af87d28a30','40637382-edb6-4876-9d30-d954fa87a336','/product-images/organic-fruits/bananas/2.jpg','Bananas image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('94712f07-ba30-4584-bb6b-84f6892b3882','40637382-edb6-4876-9d30-d954fa87a336','/product-images/organic-fruits/bananas/3.jpg','Bananas image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d1ca8b30-3948-4381-840e-22d24acf9608','40637382-edb6-4876-9d30-d954fa87a336','/product-images/organic-fruits/bananas/4.jpg','Bananas image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fb11a1d3-dffb-454f-b2ef-56b83e49963d','40637382-edb6-4876-9d30-d954fa87a336','/product-images/organic-fruits/bananas/5.jpg','Bananas image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fc4f9981-dd77-47c8-871c-6970eb48236c','5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','/product-images/organic-fruits/blueberries/1.jpg','Blueberries image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('254909da-471a-45d5-baaf-f072d191e25d','5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','/product-images/organic-fruits/blueberries/2.jpg','Blueberries image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('62295720-3d77-4778-98b5-fea8eb4d460b','5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','/product-images/organic-fruits/blueberries/3.jpg','Blueberries image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('cb0e3f53-379b-4bba-9c33-73c79f7a066c','5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','/product-images/organic-fruits/blueberries/4.jpg','Blueberries image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('cfdd0f95-14f3-4e9d-aef5-20c5b2f79238','5eb3e1a8-f8f4-4db9-9508-5c7309c231f2','/product-images/organic-fruits/blueberries/5.jpg','Blueberries image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('b20b4606-8b1c-4baf-9a95-896928452723','4c8f7cef-9baf-48c3-aa93-66d7c692d253','/product-images/organic-fruits/grapes/1.jpg','Grapes image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d48f5e3f-6595-4d25-a117-8ec7fc4f6345','4c8f7cef-9baf-48c3-aa93-66d7c692d253','/product-images/organic-fruits/grapes/2.jpg','Grapes image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('0b41e0d6-539e-4e1a-85b4-6436df56f1ee','4c8f7cef-9baf-48c3-aa93-66d7c692d253','/product-images/organic-fruits/grapes/3.jpg','Grapes image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('da9a4517-1bdc-4d28-a98b-48d591dea27e','4c8f7cef-9baf-48c3-aa93-66d7c692d253','/product-images/organic-fruits/grapes/4.jpg','Grapes image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9f3d0d04-c708-4fe5-ad70-25fec9047c53','4c8f7cef-9baf-48c3-aa93-66d7c692d253','/product-images/organic-fruits/grapes/5.jpg','Grapes image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3507aacf-1052-4def-95ca-13bbeee76178','e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','/product-images/organic-fruits/mangoes/1.jpg','Mangoes image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('6555ccc0-fcb2-436d-aa2e-fbcfc6a1e23a','e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','/product-images/organic-fruits/mangoes/3.jpg','Mangoes image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7cb112f3-578c-4ecb-88f1-fa64ce633fb8','e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','/product-images/organic-fruits/mangoes/4.jpg','Mangoes image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f607ea6f-eb8a-497f-8afe-e042822c5922','e7d6bdd5-37cc-41ae-b1f6-ccb7cad466e0','/product-images/organic-fruits/mangoes/5.jpg','Mangoes image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('cd8398fa-eac0-4b89-92f2-dfcd5aeacb37','e09384b6-2e54-45e2-8224-b4857d229f1d','/product-images/organic-fruits/oranges/1.jpg','Oranges image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f979c60e-564c-423c-ab0a-cdb5fe89cac0','e09384b6-2e54-45e2-8224-b4857d229f1d','/product-images/organic-fruits/oranges/2.jpg','Oranges image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a08e535b-cfb8-4be6-b2e2-4a08c9194dbf','e09384b6-2e54-45e2-8224-b4857d229f1d','/product-images/organic-fruits/oranges/3.jpg','Oranges image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('4b6b46f6-7840-41e3-9486-efadf3d602d8','e09384b6-2e54-45e2-8224-b4857d229f1d','/product-images/organic-fruits/oranges/4.jpg','Oranges image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('be1185f6-dc8c-4b64-a81d-414ac8c0dd8c','e09384b6-2e54-45e2-8224-b4857d229f1d','/product-images/organic-fruits/oranges/5.jpg','Oranges image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('ef668d07-b7da-4d83-ba33-0280ae039c38','1ebaf900-261c-4d6a-89be-29e4cd8b37c0','/product-images/organic-fruits/pomegranates/1.jpg','Pomegranates image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2e7649fc-643b-40c3-b283-3f017e5bec66','1ebaf900-261c-4d6a-89be-29e4cd8b37c0','/product-images/organic-fruits/pomegranates/2.jpg','Pomegranates image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c7e91935-0b34-4928-b08d-f16fe8686901','1ebaf900-261c-4d6a-89be-29e4cd8b37c0','/product-images/organic-fruits/pomegranates/3.jpg','Pomegranates image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('4ed3f1ba-0574-4c95-b99f-d1e74ec2cc1e','1ebaf900-261c-4d6a-89be-29e4cd8b37c0','/product-images/organic-fruits/pomegranates/4.jpg','Pomegranates image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2d6369de-e882-4ed4-98f5-bbbbe8d8d07b','1ebaf900-261c-4d6a-89be-29e4cd8b37c0','/product-images/organic-fruits/pomegranates/5.jpg','Pomegranates image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3e24add7-a15f-4845-89aa-f5df0e96c60e','b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','/product-images/organic-fruits/raspberries/1.jpg','Raspberries image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e64a0d55-ad88-4910-aec5-a7a113be4c2e','b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','/product-images/organic-fruits/raspberries/2.jpg','Raspberries image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('306e0015-7db4-438f-9661-9785762be85e','b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','/product-images/organic-fruits/raspberries/3.jpg','Raspberries image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('4668ccc3-277d-40d6-942e-236fe73f1115','b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','/product-images/organic-fruits/raspberries/4.jpg','Raspberries image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f2114d94-9325-4cbc-88ac-5ffdc7bc2616','b3fc7a6e-96b1-431b-8819-dd5d8c1a6f94','/product-images/organic-fruits/raspberries/5.jpg','Raspberries image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('5cb81edc-0002-4661-8cb4-df02672ded03','81fa9fab-55ee-4f33-97a5-8370f785a1e2','/product-images/organic-fruits/strawberries/1.jpg','Strawberries image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e28195d0-9b56-4535-a337-85e0e5ff235b','81fa9fab-55ee-4f33-97a5-8370f785a1e2','/product-images/organic-fruits/strawberries/2.jpg','Strawberries image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8f10b4b9-b732-4ca5-9954-7dc74b211168','81fa9fab-55ee-4f33-97a5-8370f785a1e2','/product-images/organic-fruits/strawberries/3.jpg','Strawberries image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8ca34cb6-508f-41e1-b0b7-f913544e43d5','81fa9fab-55ee-4f33-97a5-8370f785a1e2','/product-images/organic-fruits/strawberries/4.jpg','Strawberries image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e18a14ac-1286-401b-a636-f6b2b3951759','81fa9fab-55ee-4f33-97a5-8370f785a1e2','/product-images/organic-fruits/strawberries/5.jpg','Strawberries image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('1cc68edb-42d3-4b63-b833-ed1fe450a56d','c2b7565e-d21d-4a62-814c-8154eb091b6d','/product-images/organic-grains-cereals/brown-rice/1.jpg','Brown rice image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2170333f-eabb-41b6-8e82-ae176c238b4b','c2b7565e-d21d-4a62-814c-8154eb091b6d','/product-images/organic-grains-cereals/brown-rice/2.jpg','Brown rice image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('5566ef54-b68e-4b10-90e2-409abba20880','c2b7565e-d21d-4a62-814c-8154eb091b6d','/product-images/organic-grains-cereals/brown-rice/3.jpg','Brown rice image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('70721216-33b1-4f77-aa1e-1644e328352e','c2b7565e-d21d-4a62-814c-8154eb091b6d','/product-images/organic-grains-cereals/brown-rice/4.jpg','Brown rice image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a73f0e25-c1e8-42f7-8a2d-d02aac9648b8','c2b7565e-d21d-4a62-814c-8154eb091b6d','/product-images/organic-grains-cereals/brown-rice/5.jpg','Brown rice image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('4a41ebfe-1ec2-45b7-bbee-305371c6f97f','66b331ee-6099-40c4-866e-97e2092d5076','/product-images/organic-grains-cereals/buckwheat/1.jpg','Buckwheat image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('dd6dbc15-c70d-4805-8ec2-edefca0f64f9','66b331ee-6099-40c4-866e-97e2092d5076','/product-images/organic-grains-cereals/buckwheat/2.jpg','Buckwheat image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('24a554c5-461d-4313-9b17-6a36455a11f7','66b331ee-6099-40c4-866e-97e2092d5076','/product-images/organic-grains-cereals/buckwheat/3.jpg','Buckwheat image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('87bc5d94-ede6-491c-a4d8-9d1637b153ae','66b331ee-6099-40c4-866e-97e2092d5076','/product-images/organic-grains-cereals/buckwheat/4.jpg','Buckwheat image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e5e97743-5342-44fb-9607-e50a53c9bcc0','66b331ee-6099-40c4-866e-97e2092d5076','/product-images/organic-grains-cereals/buckwheat/5.jpg','Buckwheat image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('6ead665b-4f66-4f11-8885-1c9a48987feb','a114955e-0c49-4b92-8950-f0dcee779bca','/product-images/organic-grains-cereals/millet/1.jpg','Millet image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('94e3b4b1-7e29-476b-89b4-c7ef400e9c5d','a114955e-0c49-4b92-8950-f0dcee779bca','/product-images/organic-grains-cereals/millet/2.jpg','Millet image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('373a3f78-9e69-49d4-9105-48777f4c6996','a114955e-0c49-4b92-8950-f0dcee779bca','/product-images/organic-grains-cereals/millet/3.jpg','Millet image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('eb70b916-94a4-4f46-bcd2-792af15bfc99','a114955e-0c49-4b92-8950-f0dcee779bca','/product-images/organic-grains-cereals/millet/4.jpg','Millet image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c89e404f-ce6c-4380-9e8a-f1c56220546c','a114955e-0c49-4b92-8950-f0dcee779bca','/product-images/organic-grains-cereals/millet/5.jpg','Millet image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d5443eb1-473e-4347-b640-49a432a4fbee','4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','/product-images/organic-grains-cereals/oats/1.jpg','Oats image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2385fa3d-37c0-47f2-8087-61a223562525','4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','/product-images/organic-grains-cereals/oats/3.jpg','Oats image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8c76af56-e046-41a6-a13a-3e2046610e3b','4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','/product-images/organic-grains-cereals/oats/4.jpg','Oats image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('1117e993-2982-40f6-96f9-a337f6efff52','4f1f6570-acdb-4b75-9ebb-9b419eaca8a3','/product-images/organic-grains-cereals/oats/5.jpg','Oats image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8a2b7e81-a113-4b3c-86fa-58ea790d9999','3404a775-b1b9-42fa-a7db-dc156d966dbb','/product-images/organic-grains-cereals/quinoa/1.jpg','Quinoa image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a133900b-f8ff-43d9-a4c5-aa1394b40d43','3404a775-b1b9-42fa-a7db-dc156d966dbb','/product-images/organic-grains-cereals/quinoa/2.jpg','Quinoa image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d70732e3-b01e-423d-82ea-89ed92be1666','3404a775-b1b9-42fa-a7db-dc156d966dbb','/product-images/organic-grains-cereals/quinoa/3.jpg','Quinoa image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('cbf9932e-ed05-4acd-bf3d-d5c017a174e8','3404a775-b1b9-42fa-a7db-dc156d966dbb','/product-images/organic-grains-cereals/quinoa/4.jpg','Quinoa image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('5052da81-9fc6-4611-baf2-7c995728c7f8','3404a775-b1b9-42fa-a7db-dc156d966dbb','/product-images/organic-grains-cereals/quinoa/5.jpg','Quinoa image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f4b39500-7f2e-4d8a-a34a-38eef5795ee7','2fe16402-0147-4b6b-945b-e40baaf77147','/product-images/organic-herbs-spices/basil/1.jpg','Basil image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a8c533c8-19ca-4738-b15c-95493b0e4614','2fe16402-0147-4b6b-945b-e40baaf77147','/product-images/organic-herbs-spices/basil/2.jpg','Basil image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2d61d7cf-9a50-4342-8558-1774d85fe26f','2fe16402-0147-4b6b-945b-e40baaf77147','/product-images/organic-herbs-spices/basil/3.jpg','Basil image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('21f803f2-ba18-4863-ac98-4731ed1a9d28','2fe16402-0147-4b6b-945b-e40baaf77147','/product-images/organic-herbs-spices/basil/4.jpg','Basil image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9c1fed46-05c7-44eb-be15-1fd70f8356d6','2fe16402-0147-4b6b-945b-e40baaf77147','/product-images/organic-herbs-spices/basil/5.jpg','Basil image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d72ee4a8-100d-4a55-9b01-c9261f338446','ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','/product-images/organic-herbs-spices/cinnamon/1.jpg','Cinnamon image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fd009e35-ce9a-412e-897f-1e9592552b39','ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','/product-images/organic-herbs-spices/cinnamon/2.jpg','Cinnamon image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('b5586aef-ed9e-455d-892a-b96f31f704a7','ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','/product-images/organic-herbs-spices/cinnamon/3.jpg','Cinnamon image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a21b5705-c707-4e74-bd7a-16769689c108','ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','/product-images/organic-herbs-spices/cinnamon/4.jpg','Cinnamon image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9156dd9a-2933-4e0a-a539-2e88558fdd75','ec10adfc-e343-42b5-bca6-44ef2ffc0d0f','/product-images/organic-herbs-spices/cinnamon/5.jpg','Cinnamon image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('879c974c-4129-4201-821c-c7298160f80a','352a4f89-13e3-4fa2-9010-070a9ee70562','/product-images/organic-herbs-spices/ginger/1.jpg','Ginger image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9a07d654-9cbc-44ec-8017-0a36f0c536cf','352a4f89-13e3-4fa2-9010-070a9ee70562','/product-images/organic-herbs-spices/ginger/2.jpg','Ginger image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('6c5a4c22-d19f-407e-bc54-8d7a4cfc3f62','352a4f89-13e3-4fa2-9010-070a9ee70562','/product-images/organic-herbs-spices/ginger/3.jpg','Ginger image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3af1a051-e03e-43e6-a12f-0ca965a75fad','352a4f89-13e3-4fa2-9010-070a9ee70562','/product-images/organic-herbs-spices/ginger/4.jpg','Ginger image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('cfe27e2f-2373-4417-b94e-c3f61a82ff21','352a4f89-13e3-4fa2-9010-070a9ee70562','/product-images/organic-herbs-spices/ginger/5.jpg','Ginger image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('68cda855-a38d-4e0e-8336-2982adeacf51','006f2cc1-54f8-4ed2-9d6f-ac454592ea59','/product-images/organic-herbs-spices/oregano/1.jpg','Oregano image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('4c71a373-0853-4ab0-877e-c8be9161f403','006f2cc1-54f8-4ed2-9d6f-ac454592ea59','/product-images/organic-herbs-spices/oregano/2.jpg','Oregano image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('ddd5ac5a-a3b1-4a4b-b05a-1f60d3878d1c','006f2cc1-54f8-4ed2-9d6f-ac454592ea59','/product-images/organic-herbs-spices/oregano/3.jpg','Oregano image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('291f4de9-5b0f-417e-991a-a652e78d59d5','006f2cc1-54f8-4ed2-9d6f-ac454592ea59','/product-images/organic-herbs-spices/oregano/4.jpg','Oregano image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c2fbf83e-7026-496c-bd53-f9269b322fda','006f2cc1-54f8-4ed2-9d6f-ac454592ea59','/product-images/organic-herbs-spices/oregano/5.jpg','Oregano image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('b2822e60-1382-4e5d-923c-175bfb4a055c','33241df6-2896-4045-8d24-9c0e26b7304f','/product-images/organic-herbs-spices/turmeric/1.jpg','Turmeric image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('53bf93fb-c245-42cd-996d-8c6ee10d1f83','33241df6-2896-4045-8d24-9c0e26b7304f','/product-images/organic-herbs-spices/turmeric/2.jpg','Turmeric image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('88e02f27-481f-4fd2-bf74-d61cbc6c8c9d','33241df6-2896-4045-8d24-9c0e26b7304f','/product-images/organic-herbs-spices/turmeric/3.jpg','Turmeric image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9a6583bb-ffc3-4389-bb3a-601fa0cbdd45','33241df6-2896-4045-8d24-9c0e26b7304f','/product-images/organic-herbs-spices/turmeric/4.jpg','Turmeric image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c0e35ae5-e895-41ab-9e99-cf25d3b41f0e','33241df6-2896-4045-8d24-9c0e26b7304f','/product-images/organic-herbs-spices/turmeric/5.jpg','Turmeric image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e0fff6c2-151c-41ff-b601-6782db2be7fa','4ef271d3-d973-43e4-a23e-6d16fd944987','/product-images/organic-legumes-pulses/black-beans/1.jpg','Black beans image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('b083db5a-ea4c-4f71-b2d6-fddc7ec19e98','4ef271d3-d973-43e4-a23e-6d16fd944987','/product-images/organic-legumes-pulses/black-beans/2.jpg','Black beans image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fb004f4b-1538-483c-b48a-20bb93bb6557','4ef271d3-d973-43e4-a23e-6d16fd944987','/product-images/organic-legumes-pulses/black-beans/3.jpg','Black beans image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fe749118-030f-48d8-8b45-29e78e5847c2','4ef271d3-d973-43e4-a23e-6d16fd944987','/product-images/organic-legumes-pulses/black-beans/4.jpg','Black beans image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('38a221c4-92d2-460c-8b82-a243f0e684ef','4ef271d3-d973-43e4-a23e-6d16fd944987','/product-images/organic-legumes-pulses/black-beans/5.jpg','Black beans image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('146ed5d9-4b27-4bee-94d0-c709afc3d04b','a430db4f-8964-4a0f-bcce-7a7922e6b013','/product-images/organic-legumes-pulses/chickpeas/1.jpg','Chickpeas image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('1be3e164-91e1-4459-8418-30c9cecd52cf','a430db4f-8964-4a0f-bcce-7a7922e6b013','/product-images/organic-legumes-pulses/chickpeas/2.jpg','Chickpeas image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2e53766d-11df-40fc-9e3e-864f8caa812d','a430db4f-8964-4a0f-bcce-7a7922e6b013','/product-images/organic-legumes-pulses/chickpeas/3.jpg','Chickpeas image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c0a558c7-fd8b-41c2-8531-9b49f32635c4','a430db4f-8964-4a0f-bcce-7a7922e6b013','/product-images/organic-legumes-pulses/chickpeas/4.jpg','Chickpeas image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8641f04e-047d-4b6c-849c-bfc372e2323b','a430db4f-8964-4a0f-bcce-7a7922e6b013','/product-images/organic-legumes-pulses/chickpeas/5.jpg','Chickpeas image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7a652d50-a2ea-407c-8144-830ef509fca5','405bfb55-db4f-4aa3-ad7c-ff507b98f526','/product-images/organic-legumes-pulses/green-peas/1.jpg','Green peas image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d9d575b1-d1ee-4a90-bf11-50b29f716139','405bfb55-db4f-4aa3-ad7c-ff507b98f526','/product-images/organic-legumes-pulses/green-peas/2.jpg','Green peas image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('1e9c3f0a-5ec2-4254-97b1-e658469a821c','405bfb55-db4f-4aa3-ad7c-ff507b98f526','/product-images/organic-legumes-pulses/green-peas/3.jpg','Green peas image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('b7e81092-ff61-48a8-b495-aa8fb8061255','405bfb55-db4f-4aa3-ad7c-ff507b98f526','/product-images/organic-legumes-pulses/green-peas/4.jpg','Green peas image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e4bc70e9-1e59-4389-a3cc-f6a3acece1b8','405bfb55-db4f-4aa3-ad7c-ff507b98f526','/product-images/organic-legumes-pulses/green-peas/5.jpg','Green peas image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('ba7b5106-4f07-4cf5-ba46-2be8a93dc12c','b8817db1-18b6-404c-bb42-17d1e87afb36','/product-images/organic-legumes-pulses/kidney-beans/1.jpg','Kidney beans image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('aa14bfff-b293-4d3b-8e99-dca204a54e0b','b8817db1-18b6-404c-bb42-17d1e87afb36','/product-images/organic-legumes-pulses/kidney-beans/2.jpg','Kidney beans image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('86663f32-7fcc-4e98-8357-057a48eca1ce','b8817db1-18b6-404c-bb42-17d1e87afb36','/product-images/organic-legumes-pulses/kidney-beans/3.jpg','Kidney beans image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('46fd0f5e-d729-4cf5-b2c8-2838e3169ba3','b8817db1-18b6-404c-bb42-17d1e87afb36','/product-images/organic-legumes-pulses/kidney-beans/4.jpg','Kidney beans image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('da48ea93-9d6c-44f4-97cb-ed1adb026494','b8817db1-18b6-404c-bb42-17d1e87afb36','/product-images/organic-legumes-pulses/kidney-beans/5.jpg','Kidney beans image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('62f71ea2-49eb-49e3-b286-a4cfc9719d8f','0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','/product-images/organic-legumes-pulses/lentils/1.jpg','Lentils image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('67319173-b11f-44e6-9134-735db4a60c85','0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','/product-images/organic-legumes-pulses/lentils/2.jpg','Lentils image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('92819817-3c7c-4a90-ae4f-39ef7110c38a','0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','/product-images/organic-legumes-pulses/lentils/3.jpg','Lentils image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('bfb0141a-86c8-4e38-a7b5-516755285abe','0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','/product-images/organic-legumes-pulses/lentils/4.jpg','Lentils image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3fd32dc2-69a0-4de9-8889-bab6e0593b66','0ac4f0f9-8aae-4ef6-b4de-28622a1100c0','/product-images/organic-legumes-pulses/lentils/5.jpg','Lentils image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('28c858be-d3b0-4920-bd55-ccdad031d9a1','eda76370-d2c8-4a74-a602-910f443a2b95','/product-images/organic-nuts-seeds/almonds/1.jpg','Almonds image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8afb31cb-15a2-418b-96ed-cbf9e762f79e','eda76370-d2c8-4a74-a602-910f443a2b95','/product-images/organic-nuts-seeds/almonds/2.jpg','Almonds image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('ff24804a-d424-48e1-a62e-afcab4a8ad47','eda76370-d2c8-4a74-a602-910f443a2b95','/product-images/organic-nuts-seeds/almonds/3.jpg','Almonds image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('5d8b47f3-1e99-48f3-9895-6714bad32028','eda76370-d2c8-4a74-a602-910f443a2b95','/product-images/organic-nuts-seeds/almonds/4.jpg','Almonds image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('0e464ef5-a91a-4a6d-a9b9-8a6186948d51','eda76370-d2c8-4a74-a602-910f443a2b95','/product-images/organic-nuts-seeds/almonds/5.jpg','Almonds image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a80e89b1-b1d8-4fbd-93bc-bf2b2cd26b1a','e414a976-8ab2-4da4-ba50-b34927ae0d65','/product-images/organic-nuts-seeds/chia-seeds/1.jpg','Chia seeds image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('5208455f-e174-486c-b116-ce8b914b2c3b','e414a976-8ab2-4da4-ba50-b34927ae0d65','/product-images/organic-nuts-seeds/chia-seeds/2.jpg','Chia seeds image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('a3d839a6-94f8-4c08-9b92-254bb22ec554','e414a976-8ab2-4da4-ba50-b34927ae0d65','/product-images/organic-nuts-seeds/chia-seeds/3.jpg','Chia seeds image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9251ffb6-529b-4914-bf2d-1efe3d470438','e414a976-8ab2-4da4-ba50-b34927ae0d65','/product-images/organic-nuts-seeds/chia-seeds/4.jpg','Chia seeds image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('1aa0bf91-f93a-41f8-a5aa-9e053202b8c6','e414a976-8ab2-4da4-ba50-b34927ae0d65','/product-images/organic-nuts-seeds/chia-seeds/5.jpg','Chia seeds image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('440f4885-45db-4695-ae06-8d8f7b919efd','b5538745-c0b2-446c-b3d9-345d1e9c663b','/product-images/organic-nuts-seeds/flaxseeds/1.jpg','Flaxseeds image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('21b27497-3955-41b7-b371-dfd7f771cdf7','b5538745-c0b2-446c-b3d9-345d1e9c663b','/product-images/organic-nuts-seeds/flaxseeds/2.png','Flaxseeds image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('dd656dd8-53ae-4734-b342-2fba3b12bb11','b5538745-c0b2-446c-b3d9-345d1e9c663b','/product-images/organic-nuts-seeds/flaxseeds/3.jpg','Flaxseeds image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8d55ecfd-8be6-4243-9445-bb5114ffee4b','b5538745-c0b2-446c-b3d9-345d1e9c663b','/product-images/organic-nuts-seeds/flaxseeds/4.jpg','Flaxseeds image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2cdff2a0-c114-4aa9-9888-acfa5098f1e7','9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','/product-images/organic-nuts-seeds/pumpkin-seeds/1.jpg','Pumpkin seeds image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('907b5b7d-8cf9-47a1-8925-b547852c20e8','9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','/product-images/organic-nuts-seeds/pumpkin-seeds/2.jpg','Pumpkin seeds image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('dec0cc58-fa49-42c6-9c7d-629ff6fe8629','9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','/product-images/organic-nuts-seeds/pumpkin-seeds/3.jpg','Pumpkin seeds image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f1af2e40-3755-4281-ba16-4848a831aa17','9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','/product-images/organic-nuts-seeds/pumpkin-seeds/4.jpg','Pumpkin seeds image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('cf849110-e0d1-4083-b8fe-c4abeffebae2','9cf94514-a5e8-41dd-b96e-c2fbbebb4d3c','/product-images/organic-nuts-seeds/pumpkin-seeds/5.jpg','Pumpkin seeds image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('21c2aa29-6d81-494e-937e-0448f3f9f921','53a9112f-a7ac-44bf-a007-c968b617ea27','/product-images/organic-nuts-seeds/walnuts/1.jpg','Walnuts image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3adebd3d-9604-436d-abae-6e5ae93a279a','53a9112f-a7ac-44bf-a007-c968b617ea27','/product-images/organic-nuts-seeds/walnuts/3.jpg','Walnuts image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('58e7251b-dd7f-44f8-a2d7-94012e614c04','53a9112f-a7ac-44bf-a007-c968b617ea27','/product-images/organic-nuts-seeds/walnuts/5.jpg','Walnuts image',2,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c29fa405-6f38-4d51-8db3-1e45465ce6b0','5a90ff57-d854-4da2-9351-979a0f908b10','/product-images/organic-vegetables/beets/1.jpg','Beets image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c17cec17-4432-4efa-9ebc-c1c11d3c281b','5a90ff57-d854-4da2-9351-979a0f908b10','/product-images/organic-vegetables/beets/2.jpg','Beets image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('44cb0255-6ecf-4829-8f8a-0128b85ae5cc','5a90ff57-d854-4da2-9351-979a0f908b10','/product-images/organic-vegetables/beets/3.jpg','Beets image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('ed800e31-2af0-49dd-8cd1-a03eb16a8614','5a90ff57-d854-4da2-9351-979a0f908b10','/product-images/organic-vegetables/beets/4.jpg','Beets image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('bc0638e6-2fde-4b89-8b70-595cbc33288a','5a90ff57-d854-4da2-9351-979a0f908b10','/product-images/organic-vegetables/beets/5.jpg','Beets image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('bc6820c0-a9aa-4b62-8282-03e436e62213','dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','/product-images/organic-vegetables/bell-peppers/1.jpg','Bell peppers image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('b95ec9eb-18a8-4255-8ee6-a8f8630a09ba','dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','/product-images/organic-vegetables/bell-peppers/3.jpg','Bell peppers image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('e367253c-27da-4121-a48f-8a2908b41aa7','dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','/product-images/organic-vegetables/bell-peppers/4.jpg','Bell peppers image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7fda4c02-0551-4bd2-9468-dbdeffcd24ee','dffbf1d4-aaf9-4832-b4e8-9e0ac723c9e6','/product-images/organic-vegetables/bell-peppers/5.jpg','Bell peppers image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('ea1213f1-6afe-44a8-9a2d-facd6574b44b','13cb79e0-ff9c-40bf-b4c4-178faf796b38','/product-images/organic-vegetables/broccoli/1.jpg','Broccoli image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('19ad878f-246a-4075-82f8-8e1a2fbb5369','13cb79e0-ff9c-40bf-b4c4-178faf796b38','/product-images/organic-vegetables/broccoli/2.jpg','Broccoli image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8a55ddf9-73e7-4144-b7a7-39f722528ee0','13cb79e0-ff9c-40bf-b4c4-178faf796b38','/product-images/organic-vegetables/broccoli/3.jpg','Broccoli image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('20bee0b7-0f68-44a4-b18b-9798e1894349','13cb79e0-ff9c-40bf-b4c4-178faf796b38','/product-images/organic-vegetables/broccoli/4.jpg','Broccoli image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8a25b3ed-a42d-4c13-90a8-efd5e086b3ce','13cb79e0-ff9c-40bf-b4c4-178faf796b38','/product-images/organic-vegetables/broccoli/5.jpg','Broccoli image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('209c29cd-075d-41bb-9d12-762555e19cca','d89c0187-8b38-4efb-bcd2-44188a15cb5d','/product-images/organic-vegetables/brussels-sprouts/1.jpg','Brussels sprouts image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('58a30318-c0fa-4707-86b5-f896da7c2f94','d89c0187-8b38-4efb-bcd2-44188a15cb5d','/product-images/organic-vegetables/brussels-sprouts/2.jpg','Brussels sprouts image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('fded4aab-a2e6-4cc2-9ba7-da873dda0dbc','d89c0187-8b38-4efb-bcd2-44188a15cb5d','/product-images/organic-vegetables/brussels-sprouts/3.jpg','Brussels sprouts image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('b2dae0ca-c6b2-49cd-bfd9-a6b6160182fe','d89c0187-8b38-4efb-bcd2-44188a15cb5d','/product-images/organic-vegetables/brussels-sprouts/5.jpg','Brussels sprouts image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('20493d8f-85e7-4675-aec1-25d0bef8b7e2','59873b7f-712c-42f0-a7bd-67f1568c0a92','/product-images/organic-vegetables/carrots/1.jpg','Carrots image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('554b389b-f02a-43fa-8de7-4a905760a711','59873b7f-712c-42f0-a7bd-67f1568c0a92','/product-images/organic-vegetables/carrots/2.jpg','Carrots image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('21c84d98-17f9-40a6-8afd-83d9bbe0acbe','59873b7f-712c-42f0-a7bd-67f1568c0a92','/product-images/organic-vegetables/carrots/3.jpg','Carrots image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('824be114-3fa6-48cf-9069-e3992ca6819a','59873b7f-712c-42f0-a7bd-67f1568c0a92','/product-images/organic-vegetables/carrots/4.jpg','Carrots image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('49fef392-f247-4639-8929-8216d7f93199','59873b7f-712c-42f0-a7bd-67f1568c0a92','/product-images/organic-vegetables/carrots/5.jpg','Carrots image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9aeea160-698f-4f00-a33a-bca2a6bdd698','da6d6abb-138d-45a1-9ec5-6cb5f997f297','/product-images/organic-vegetables/cauliflower/1.jpg','Cauliflower image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('c68991a6-0f02-4f33-8a5b-2af517e29cc4','da6d6abb-138d-45a1-9ec5-6cb5f997f297','/product-images/organic-vegetables/cauliflower/2.jpg','Cauliflower image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f86f2a17-b364-4caa-a41a-7932d8bfb212','da6d6abb-138d-45a1-9ec5-6cb5f997f297','/product-images/organic-vegetables/cauliflower/3.jpg','Cauliflower image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9c43ff2c-9a29-4239-951d-0cee274bd5a7','da6d6abb-138d-45a1-9ec5-6cb5f997f297','/product-images/organic-vegetables/cauliflower/4.jpg','Cauliflower image',3,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('565c18a7-6e84-4084-a0b1-f6e41beb5933','19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','/product-images/organic-vegetables/kale/1.jpg','Kale image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('39221bff-87fa-4701-8b3b-919f530c9bba','19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','/product-images/organic-vegetables/kale/2.jpg','Kale image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d626e4db-ed6d-4b9b-ba8f-d1a95a06e290','19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','/product-images/organic-vegetables/kale/3.jpg','Kale image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('578800dd-9ce7-48ac-b623-dffa96235c14','19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','/product-images/organic-vegetables/kale/4.jpg','Kale image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7d08a851-e225-4951-a39c-2180e03a4b5c','19c0c8ee-5a44-4ec7-92e8-0aec6c491fb0','/product-images/organic-vegetables/kale/5.jpg','Kale image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3c15ceef-c9f0-456f-ac38-f5c00674b236','ab8d6bc4-68af-4103-a778-428788914fa2','/product-images/organic-vegetables/spinach/1.png','Spinach image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('cfacc472-b400-4a36-b69c-c576d3e79a25','ab8d6bc4-68af-4103-a778-428788914fa2','/product-images/organic-vegetables/spinach/2.jpg','Spinach image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('9f9f1745-8175-42be-858d-b03a5655beac','ab8d6bc4-68af-4103-a778-428788914fa2','/product-images/organic-vegetables/spinach/3.jpg','Spinach image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('7d5a2673-cd36-47f4-bb1a-b98b3a441a4d','ab8d6bc4-68af-4103-a778-428788914fa2','/product-images/organic-vegetables/spinach/4.jpg','Spinach image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f6cdbd68-5e0b-48e0-bb79-7dacd9f2ff45','ab8d6bc4-68af-4103-a778-428788914fa2','/product-images/organic-vegetables/spinach/5.png','Spinach image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8657d3e1-0fdd-4845-912b-dd44565be184','a274943b-6f07-450f-9b3d-c9a79d80b232','/product-images/organic-vegetables/sweet-potato/1.jpg','Sweet potato image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8e2a9e4f-a3f0-4099-bc34-5702421b54c5','a274943b-6f07-450f-9b3d-c9a79d80b232','/product-images/organic-vegetables/sweet-potato/2.jpg','Sweet potato image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('2b8b0d4e-aff7-4f8b-b70e-d4570a12ba02','a274943b-6f07-450f-9b3d-c9a79d80b232','/product-images/organic-vegetables/sweet-potato/3.jpg','Sweet potato image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('d9b7d219-b299-43db-83f3-d39d28998c93','a274943b-6f07-450f-9b3d-c9a79d80b232','/product-images/organic-vegetables/sweet-potato/4.jpg','Sweet potato image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('8d09a4f4-6012-45cb-89ff-baf49859e348','a274943b-6f07-450f-9b3d-c9a79d80b232','/product-images/organic-vegetables/sweet-potato/5.jpg','Sweet potato image',4,0,SYSUTCDATETIME());

  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('3b33c7ab-0959-4aea-8ca3-254067e0aa00','93b85e86-bd44-46b2-90dc-2872cdb9f071','/product-images/organic-vegetables/tomatoes/1.jpg','Tomatoes image',0,1,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('86d96afb-bbe2-45a2-b454-b21de03e0653','93b85e86-bd44-46b2-90dc-2872cdb9f071','/product-images/organic-vegetables/tomatoes/2.jpg','Tomatoes image',1,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f976cdfa-6bed-4b35-a43f-7e1155642c95','93b85e86-bd44-46b2-90dc-2872cdb9f071','/product-images/organic-vegetables/tomatoes/3.jpg','Tomatoes image',2,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('f4dbd3f8-a85d-4c8a-927f-ba796dccd128','93b85e86-bd44-46b2-90dc-2872cdb9f071','/product-images/organic-vegetables/tomatoes/4.jpg','Tomatoes image',3,0,SYSUTCDATETIME());
  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('92641f5c-9979-4324-a1f5-49ea847a9117','93b85e86-bd44-46b2-90dc-2872cdb9f071','/product-images/organic-vegetables/tomatoes/5.jpg','Tomatoes image',4,0,SYSUTCDATETIME());

  COMMIT TRANSACTION;
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
  THROW;
END CATCH

