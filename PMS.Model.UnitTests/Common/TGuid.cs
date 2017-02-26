using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Model.UnitTests.Common
{
    public struct TGuid
    {
        const string Beginning = "00000000-0000-4342-";
        const string Ending = "-000000000000";

        public int Id { get; set; }

        public TGuid(int id)
        {
            Id = id;
        }

        public TGuid(Guid guid)
        {
            string guidString = guid.ToString();
            bool isArgumentValid = guidString.StartsWith(Beginning) && guidString.EndsWith(Ending);
            if (!isArgumentValid)
                throw new ArgumentException("Invalid guid argument");

            string idString = guidString.Substring(Beginning.Length, 4);
            Id = int.Parse(idString);
        }

        public Guid ToGuid()
        {
            string number = ToString();
            return new Guid($"{Beginning}{number}{Ending}");
        }

        public override string ToString()
        {
            return Id.ToString("0000");
        }

        public override bool Equals(object obj)
        {
            return this == (TGuid)obj;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(TGuid g1, TGuid g2)
        {
            return g1.Id == g2.Id;
        }

        public static bool operator !=(TGuid g1, TGuid g2)
        {
            return g1.Id != g2.Id;
        }

        public static implicit operator Guid(TGuid g)
        {
            return g.ToGuid();
        }

        public static List<TGuid> FromList(params int[] ids)
        {
            return FromList(ids.ToList());
        }

        public static List<TGuid> FromList(IEnumerable<int> ids)
        {
            return ids.Select(id => new TGuid(id)).ToList();
        }

        public static ICollection<TGuid> FromRange(int start, int count)
        {
            return FromList(Enumerable.Range(start, count));
        }
    }
}
