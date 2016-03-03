using ProtoBuf;

namespace Guybrush.ProtoMembers.Tests
{
    public class Test
    {
        [ProtoMember(1)]
        public bool B { get; set; }

        [ProtoMember({selstart}0{caret}{selend}})]
		public string Str { get; set; }
	}
}