using System;

namespace SmartHome.Core.Dto
{
    public class NodeDataRecordDto
    {
        public object Value { get; }
        public DateTime TimeStamp { get; }

        public NodeDataRecordDto(DateTime timeStamp, object value)
        {
            TimeStamp = timeStamp;
            Value = value;
        }

        public NodeDataRecordDto()
        {
        }
    }
}
