USE [HospitalManagement]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 11/3/2020 3:50:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LanguageName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Specialities]    Script Date: 11/3/2020 3:50:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specialities](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SpecialityName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Specialities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Languages] ON 

INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (1, N'CHINESE')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (2, N'SPANISH')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (3, N'ENGLISH')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (4, N'HINDI')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (5, N'ARABIC')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (6, N'PORTUGUESE')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (7, N'BENGALI')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (8, N'RUSSIAN')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (9, N'JAPANESE')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (10, N'LAHNDA')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (11, N'JAVANESE')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (12, N'GERMAN')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (13, N'KOREAN')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (14, N'FRENCH')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (15, N'TELUGU')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (16, N'TURKISH')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (17, N'TAMIL')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (18, N'VIETNAMESE')
INSERT [dbo].[Languages] ([Id], [LanguageName]) VALUES (19, N'URDU')
SET IDENTITY_INSERT [dbo].[Languages] OFF
GO
SET IDENTITY_INSERT [dbo].[Specialities] ON 

INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (1, N'Allergists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (2, N'Anesthesiologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (3, N'Cardiologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (4, N'Colon and Rectal Surgeons')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (5, N'Critical Care Medicine Specialists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (6, N'Dermatologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (7, N'Endocrinologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (8, N'Emergency Medicine Specialists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (9, N'Family Physicians')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (10, N'Gastroenterologists
')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (11, N'Geriatric Medicine Specialists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (12, N'Hematologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (13, N'Hospice and Palliative Medicine Specialists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (14, N'Infectious Disease Specialists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (15, N'Internists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (16, N'Medical Geneticists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (17, N'Nephrologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (18, N'Neurologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (19, N'Obstetricians and Gynecologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (20, N'Oncologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (21, N'Ophthalmologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (22, N'Osteopaths')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (23, N'Otolaryngologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (24, N'Pathologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (25, N'Pediatricians')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (26, N'Physiatrists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (27, N'Plastic Surgeons')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (28, N'Podiatrists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (29, N'Preventive Medicine Specialists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (30, N'Psychiatrists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (31, N'Pulmonologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (32, N'Radiologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (33, N'Rheumatologists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (34, N'Sleep Medicine Specialists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (35, N'Sports Medicine Specialists')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (36, N'General Surgeons')
INSERT [dbo].[Specialities] ([Id], [SpecialityName]) VALUES (37, N'Urologists')
SET IDENTITY_INSERT [dbo].[Specialities] OFF
GO
