use canalplus_collecte

-- SUPPRESSION D'UN USER --
declare  @id uniqueidentifier
select @id = Id from dbo.Users where Email = 'simon.budin@gmail.com'
delete from dbo.AnswerChoices where UserId = @id
delete from dbo.InstantGagnants where UserId = @id
delete from dbo.Users where Email = 'simon.budin@gmail.com'


-- SELECTION D'UN USER --
select * from dbo.Users where Email = 'simon.budin@gmail.com'





-- BUNDLES --

SELECT [BundleId],[Date],[Status],[NbInscriptions],[NbOk],[NbKo],[NbRetoursCanal] FROM [canalplus_collecte].[dbo].[Bundles]

SELECT [BundleFileId],[Type],[FileName],[CreationDate],[Bundle_BundleId]  FROM [canalplus_collecte].[dbo].[BundleFiles]
