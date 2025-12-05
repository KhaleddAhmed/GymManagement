namespace GymManagementDAL.Entities
{
	public class MemberEntity : GymUser
	{
		public string Photo { get; set; } = null!;
		public HealthRecordEntity HealthRecord { get; set; } = null!;
		public ICollection<BookingEntity> MemberSessions { get; set; } = null!;

		public ICollection<MembershipEntity> MemberPlans { get; set; } = null!;
	}
}
