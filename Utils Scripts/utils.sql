use canalplus_collecte

-- SUPPRESSION D'UN USER --
declare  @id uniqueidentifier
select @id = Id from dbo.Users where Email = 'simon.budin@gmail.com' or Email = 'budsi_fr@hotmail.com'
delete from dbo.AnswerChoices where UserId in (select @id)
delete from dbo.InstantGagnants where UserId in (select @id)
delete from dbo.Users where Email = 'budsi_fr@hotmail.com' or Email = 'budsi_fr@hotmail.com'


-- SELECTION D'UN USER --
select * from dbo.Users where Email = 'simon.budin@gmail.com' or Email = 'budsi_fr@hotmail.com'

-- BUNDLES --
SELECT [BundleId],[Date],[Status],[NbInscriptions],[NbOk],[NbKo],[NbRetoursCanal] FROM [canalplus_collecte].[dbo].[Bundles]
SELECT [BundleFileId],[Type],[FileName],[CreationDate],[Bundle_BundleId]  FROM [canalplus_collecte].[dbo].[BundleFiles]


DELETE FROM [canalplus_collecte].[dbo].[BundleFiles]
DELETE FROM [canalplus_collecte].[dbo].[Bundles]



