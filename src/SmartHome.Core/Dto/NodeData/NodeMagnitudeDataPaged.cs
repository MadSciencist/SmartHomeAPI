using SmartHome.Core.Infrastructure;

namespace SmartHome.Core.Dto.NodeData
{
    public class NodeMagnitudeDataPaged : NodeMagnitudeData
    {
        private PagedResult<NodeMagnitudeRecord> _pagedData;
        public PagedResult<NodeMagnitudeRecord> PagedData
        {
            get => _pagedData;
            set
            {
                _pagedData = value;
                Data = value.Results;
            }
        }
    }
}
