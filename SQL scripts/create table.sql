create table npGalleryContentType
(
	id int not null constraint pk_npGalleryContentType primary key identity(1,10),
	typeName nvarchar(150) not null
)

insert into npGalleryContentType (typeName) values ('video')
insert into npGalleryContentType (typeName) values ('image')

create table npGalleryContent
(
	id int not null constraint pk_npGalleryContent Primary key identity(1,10),
	npGalleryContentTypeId int not null constraint fk_npGalleryContent_npGalleryContentType foreign key references npGalleryContentType(id),
	ContainingFolderName nvarchar(300) null,
	ImageName nvarchar(300) null,
	videoId int null
)

create table npGallery
(
	id int not null constraint pk_npGallery  Primary key identity(1,10),
	sportId int not null constraint fk_npGallery_zonesports Foreign key references zonesports,
	categoryId int not null constraint fk_npGallery_zonecategories Foreign key references zonecategories,
	npGalleryContentThumbnailId int null constraint fk_Thumbnail_npGallery_npGalleryContent Foreign key references npGalleryContent(id),
	npGalleryContentMainContentId int null constraint fk_MainContent_npGallery_npGalleryContent Foreign key references npGalleryContent(id),
	title nvarchar(200) not null,
	synopsis nvarchar(1000) null,
	active bit constraint df_npGallery_active default 0 not null,
	friendlyName nvarchar(200) not null,
	tag nvarchar(100) null,
	script nvarchar(500) null,
	featured bit constraint df_npGallery_featured default 0 not null,
	galleryRank int not null,
	dateCreated date constraint df_npGallery_created default getDate() not null,
	dateModified date not null,
	createdByUserId int not null constraint fk_Gallery_createdUser foreign key references dbo.zoneusers
)

create table npGalleryItem
(
	id int not null constraint pk_npGalleryItem primary key identity(1,10),
	npGalleryContentThumbnailId int null constraint fk_Thumbnail_npGalleryItem_npGalleryContent Foreign key references npGalleryContent(id),
	npGalleryContentMainContentId int null constraint fk_MainContent_npGalleryItem_npGalleryContent Foreign key references npGalleryContent(id),
	npGalleryId int not null constraint fk_MainContent_npGalleryItem_npGallery Foreign key references npGallery(id),
	title nvarchar(200) not null,
	synopsis nvarchar(1000) null,
	itemRank int not null,
	active bit constraint df_npGalleryItem_active default 0 not null,
	dateCreated date constraint df_npGalleryItem_created default getDate() not null,
	dateModified date not null,
	createdByUserId int not null constraint fk_GalleryItem_createdUser foreign key references dbo.zoneusers(id)
)

alter table npGallery add npGalleryContentBackgroundId int null constraint fk_Background_npGalleryItem_npGalleryContent Foreign key references npGalleryContent(id)