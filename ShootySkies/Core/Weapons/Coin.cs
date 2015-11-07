﻿using System;
using System.Threading.Tasks;
using Urho;

namespace ShootySkies
{
	public class Coin : Weapon
	{
		public override TimeSpan ReloadDuration => TimeSpan.FromSeconds(0.1f);

		public override int Damage => 0;

		public Coin(Context context) : base(context) {}

		protected override async Task OnFire(bool byPlayer)
		{
			var cache = Application.ResourceCache;
			var node = CreateRigidBullet(byPlayer);
			var model = node.CreateComponent<StaticModel>();
			model.Model = cache.GetModel(Assets.Models.Coin);
			model.SetMaterial(cache.GetMaterial(Assets.Materials.Coin));
			node.SetScale(1);
			node.Rotation = new Quaternion(-40, 0, 0);
			await node.RunActionsAsync(
				new Urho.Parallel(
					new MoveBy(duration: 3f, position: new Vector3(0, 10 * (byPlayer ? 1 : -1), 0)),
					new RotateBy(duration: 3f, deltaAngleX: 0, deltaAngleY: 360 * 5, deltaAngleZ: 0)));
			node.Remove();
		}

		public override void OnHit(Aircraft target, bool killed, Node bulletNode)
		{
			base.OnHit(target, killed, bulletNode);
			((ShootySkiesGame)Application).OnCoinCollected();
		}
	}
}
