using FamilyTree.Core;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public class BLRelationType : BLCommon<RelationType>, IRelationTypeService
    {
        public BLRelationType(IRelationTypeRepository dbContext) : base(dbContext)
        {
        }


        public override Response ValidationBeforePreSave(RelationType relationType)
        {
            Response objResponse = new Response();
            bool isDuplicate = HasDuplicate(
                nameof(RelationType),
                relationType,
                nameof(RelationType.RelationTypeCode),
                nameof(RelationType.RelationTypeDescription));

            if (isDuplicate)
            {
                objResponse.IsError = true;
                objResponse.MessageCode = MessageCode.E002.ToString().Replace("~{handler}~", "Relation Type");
            }
            return objResponse;

        }


    }
}
