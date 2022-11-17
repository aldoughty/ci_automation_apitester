namespace ci_automation_apitester.Queries
{
    public class DataTaggingQueries
    {
        public string QueryAdminTagTypes(string orgId)
        {
            //Returns DataTaggingTypes in taggingUI.TenantDataTaggingType by OrgId for GET Admin
            var results = $@"select (select a.Id, a.Name from taggingUI.DataTaggingType a 
                                join taggingUI.TenantDataTaggingType b on a.Id = b.DataTaggingTypeId 
                                join dbo.TenantOrgMapping c on b.TenantId = c.TenantId 
                                where c.OrgID = '" + orgId + "' for json path, without_array_wrapper) as taggingTypes";

            return results;
        }
        public string QueryAdminDataModels(string orgId)
        {
            //Returns DataModels in taggingUI.DataModel by OrgId for GET Admin
            var results = $@"select (select d.Id, concat(d.Name, ' - ', d.Version) as name, d.Description, d.Active " +
                          "from taggingUI.DataModel d " +
                          "join  taggingUI.TenantDataModel e on d.Id = e.DataModelId " +
                          "join dbo.TenantOrgMapping f on e.TenantId = f.TenantId " +
                          "where a.id = d.id and f.OrgID = '" + orgId + "' " +
                          "for json path, without_array_wrapper) as dataModels " +
                          "from taggingUI.DataModel a " +
                          "join  taggingUI.TenantDataModel b on a.Id = b.DataModelId " +
                          "join dbo.TenantOrgMapping c on b.TenantId = c.TenantId " +
                          "where c.OrgID = '" + orgId + "'";

            return results;
        }
        public string QueryAdminConfigs(string orgId)
        {
            //Returns Configs in taggingUI.TenantSaveScheme by OrgId for GET Admin
            var results = $@"select a.DataModelId as dataModel, (select c.Id, b.OrgId, c.Active, c.StartYear, c.EndYear, " +
                          "c.DataTaggingTypeId, c.DataModelId, json_query('[]') as columnIds from taggingUI.TenantSaveScheme c " +
                          "join dbo.TenantOrgMapping b on c.TenantId = b.TenantId " +
                          "where a.DataModelId = c.DataModelId and c.Active = 1 and b.OrgID = '" + orgId + "' " +
                          "for json path, INCLUDE_NULL_VALUES, without_array_wrapper) as configs " +
                          "from taggingUI.TenantSaveScheme a " +
                          "join dbo.TenantOrgMapping b on a.TenantId = b.TenantId " +
                          "where a.Active = 1 and b.OrgID = '" + orgId + "'" +
                          "group by a.DataModelId";

            return results;
        }
        public string QueryAdminDimensions(string orgId)
        {
            //Returns Dimensions in taggingUI.Dimension by OrgId for GET Admin
            var results = $@"SELECT CONVERT(varchar(64), dim.Id) as Id_String, " +
                          "(select distinct  dim2.id, dim.name, null as description, dim.active " +
                          "FROM taggingUI.TenantDataModel m " +
                          "JOIN taggingUI.DataModel d on m.DataModelId = d.Id " +
                          "JOIN taggingUI.TenantDataModelDimension tdm on m.Id = tdm.TenantDataModelId " +
                          "JOIN taggingUI.Dimension dim2 on tdm.DimensionId = dim.Id " +
                          "JOIN dbo.TenantOrgMapping tm on m.TenantId = tm.TenantId " +
                          "WHERE tm.OrgID = '" + orgId + "' and dim.Id = dim2.Id for json path, " +
                          "INCLUDE_NULL_VALUES, without_array_wrapper) as dimensions " +
                          "FROM taggingUI.Dimension dim " +
                          "ORDER BY Id_String DESC";

            return results;
        }
        public string QueryAdminColumns()
        {
            //Returns Columns in taggingUI.DataModelField for GET Admin
            var results = $@"select (select id, name, null as description, active from taggingUI.DataModelField 
                          for json path, INCLUDE_NULL_VALUES, without_array_wrapper) as columns";

            return results;
        }
        public string QueryAllColumnDefinitions()
        {
            //Returns all ColumnDefinitions in taggingUI.ColumnDefinitions (for GET All of ColumnDefinitions)
            var results = $@"SELECT (SELECT cd.Id, cd.Field as columnField, cd.Header as columnHeaderName, 
                           ct.Name as type, cd.ColumnTypeId, cd.Filterable, cd.Editable, 
                           cd.NotEditableWhenSelected as notEditableWhenTagRuleSelected, cd.ValueOptions, 
                           cd.IsCurrency 
                           FROM taggingUI.ColumnDefinition cd
                           JOIN taggingUI.ColumnType ct
                           ON cd.ColumnTypeId = ct.Id
                           FOR JSON PATH, INCLUDE_NULL_VALUES) as ColumnDefinitions";

            return results;
        }
        public string QueryEachColumnDefinition()
        {
            //Returns each ColumnDefinitionId in taggingUI.ColumnDefinition (for GET of specific ColumnDefinition)
            var results = $@"SELECT cd.Id as columnDefinitionId, 
                          (SELECT cd2.Id, cd2.Field as columnField, cd2.Header as columnHeaderName, 
                          ct.Name as type, cd2.ColumnTypeId, cd2.Filterable, cd2.Editable, 
                          cd2.NotEditableWhenSelected as notEditableWhenTagRuleSelected, cd2.ValueOptions, 
                          cd2.IsCurrency 
                          FROM taggingUI.ColumnDefinition cd2
                          JOIN taggingUI.ColumnType ct
                          ON cd.ColumnTypeId = ct.Id 
                          WHERE cd2.Id = cd.Id
                          FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER) 
                          AS ColumnDefinition
                          FROM taggingUI.ColumnDefinition cd";

            return results;
        }
        public string QueryAllColumnTypes()
        {
            //Returns all ColumnTypes in taggingUI.ColumnTypes (for GET all of ColumnTypes)
            var results = $@"SELECT (SELECT Id, Name, Description, Active
                          FROM taggingUI.ColumnType
                          FOR JSON PATH, INCLUDE_NULL_VALUES) as ColumnTypes";

            return results;
        }
        public string QueryEachColumnType()
        {
            //Returns each ColumnTypeId in taggingUI.ColumnType (for GET of specific ColumnDefinition)
            var results = $@"SELECT ct.Id as columnTypeId, 
                          (SELECT ct2.Id, ct2.Name, ct2.Description, ct2.Active
                          FROM taggingUI.ColumnType ct2
                          WHERE ct2.Id = ct.Id
                          FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER) 
                          AS ColumnType
                          FROM taggingUI.ColumnType ct";

            return results;
        }
        public string QuerySeasonByEntityId()
        {
            //Returns each ColumnTypeId in taggingUI.ColumnType (for GET of specific ColumnDefinition)
            var results = $@"SELECT distinct es.entityid as entityId, 
                          (SELECT s.Id, s.Name, s.Description, s.Active
                          FROM taggingUI.Season s 
                          JOIN taggingUI.EntitySeason es2
                          ON s.id = es2.seasonid
                          WHERE es.entityid = es2.entityid FOR JSON PATH) AS seasons
                          FROM taggingUI.EntitySeason es";

            return results;
        }
        public string QueryTenantSchemesByOrgId()
        {
            //Returns Orgs with dbo.TenantOrgMapping.Active = 1, dbo.Tenant.Active = 1, and having records in taggingUI.TenantSaveScheme
            //where taggingUI.TenantSaveScheme.Active = 1 (for GET by specific orgId)
            var results = $@"SELECT distinct org.orgId as OrgId, 
                          (SELECT sch.id, org2.orgId, sch.active, sch.startYear, sch.endYear, sch.dataTaggingTypeId, 
                          sch.dataModelId, json_query('[]') as columnIds
                          FROM taggingUI.TenantSaveScheme sch 
                          JOIN dbo.TenantOrgMapping org2
                          ON sch.TenantId = org2.TenantId
                          WHERE org.orgId = org2.orgId and sch.Active = 1 FOR JSON PATH, INCLUDE_NULL_VALUES) AS Schemes
                          FROM dbo.TenantOrgMapping org 
                          JOIN dbo.Tenant ten
                          ON org.TenantId = ten.TenantID
                          WHERE org.Active = 1 AND ten.Active = 1 and EXISTS (SELECT sch.id, org2.orgId, sch.active, 
                          sch.startYear, sch.endYear, sch.dataTaggingTypeId, sch.dataModelId, json_query('[]') as columnIds
                          FROM taggingUI.TenantSaveScheme sch 
                          JOIN dbo.TenantOrgMapping org2
                          ON sch.TenantId = org2.TenantId
                          WHERE org.orgId = org2.orgId and sch.Active = 1)";

            return results;
        }
        public string QueryAllTenantSchemes()
        {
            //Returns all Schemes in taggingUI.TenantSaveScheme (for GET all)
            var results = $@"SELECT (SELECT sch2.id, org.orgId, sch2.active, sch2.startYear, sch2.endYear, 
                          sch2.dataTaggingTypeId, sch2.dataModelId, json_query('[]') as columnIds
                          FROM taggingUI.TenantSaveScheme sch2
                          JOIN dbo.TenantOrgMapping org
                          ON sch2.TenantId = org.TenantId
                          FOR JSON PATH, INCLUDE_NULL_VALUES) as Schemes";

            return results;
        }
        public string QueryTenantSchemeById()
        {
            //Returns each SchemeId in taggingUI.TenantSaveScheme (for GET of specific scheme)
            var results = $@"SELECT sch.Id as SchemeId, 
                          (SELECT sch2.id, org.orgId, sch2.active, sch2.startYear, sch2.endYear, sch2.dataTaggingTypeId, 
                          sch2.dataModelId, json_query('[]') as columnIds
                          FROM taggingUI.TenantSaveScheme sch2 
                          JOIN dbo.TenantOrgMapping org
                          ON sch2.TenantId = org.TenantId
                          WHERE sch2.Id = sch.Id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER) AS Scheme
                          FROM taggingUI.TenantSaveScheme sch";

            return results;
        }
        public string QueryDataModels()
        {
            //Returns DataModels in taggingUI.DataModel for GET DataModels
            var results = $@"SELECT (SELECT Version, Id, Name, Description, Active FROM taggingUI.DataModel FOR JSON PATH) as DataModels";

            return results;
        }
        public string QueryDimensions()
        {
            //Returns Dimensions in taggingUI.Dimension for GET Dimension
            var results = $@"SELECT (SELECT Id, Name, Description, Active FROM taggingUI.Dimension FOR JSON PATH) as Dimensions";

            return results;
        }
        public string QueryHierarchies()
        {
            //Returns Hierarchies in taggingUI.Hierarchy for GET Hierarchy
            var results = $@"SELECT (SELECT Id, Name, Description, Active FROM taggingUI.Hierarchy FOR JSON PATH) as Hierarchies";

            return results;
        }
        public string QueryTagRules()
        {
            //Returns TagRules for each TagRuleFilter in taggingUI.TagRule (for GET of TagRule)
            var results = $@"SELECT trf2.Id as TagRuleFilterId, (SELECT trf.Id, trf.TagRuleId, trf.ColumnDefinitionId, cd.Field as Name, 
                            ct.Name as Type, trf.Operator, trf.Value FROM taggingUI.TagRuleFilter trf
                            JOIN taggingUI.TagRule tr
                            ON trf.TagRuleId = tr.Id
                            JOIN taggingUI.ColumnDefinition cd
                            ON trf.ColumnDefinitionId = cd.Id
                            JOIN taggingUI.ColumnType ct
                            ON cd.ColumnTypeId = ct.Id
                            WHERE trf2.TagRuleId = tr.Id
                            FOR JSON PATH) as TagRules
                            FROM taggingUI.TagRuleFilter trf2
                            JOIN taggingUI.TagRule tr2
                            ON trf2.TagRuleId = tr2.Id";

            return results;
        }
    }
}

