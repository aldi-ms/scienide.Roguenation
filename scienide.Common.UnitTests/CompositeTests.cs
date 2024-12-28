using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;

namespace scienide.Common.UnitTests
{
    public class CompositeTests
    {
        internal class Comp : GameComposite
        {
            public Comp(Point pos) : base(pos)
            {
            }
        }

        internal class Component : GameComponent
        {

        }

        [Fact]
        public void AddAndGetComponent()
        {
            var composite = new Comp(Point.Zero);
            var component = new Component();
            composite.AddComponent(component);

            var result = composite.TryGetComponent<Component>(out var x);
            Assert.True(result);
            Assert.Equal(component, x);
        }

        [Fact]
        public void AddAndRemoveComponent()
        {
            var composite = new Comp(Point.Zero);
            var component = new Component();
            composite.AddComponent(component);
            composite.AddComponent(new Terrain(new Glyph('.')));

            if (composite.TryGetComponent<Terrain>(out var t))
            {
                var result = composite.RemoveComponent(t);
                Assert.True(result);
            }
            else
            {
                Assert.Fail("Unsuccessful get of component!");
            }

            if (composite.TryGetComponent<Component>(out var x))
            {
                var result = composite.RemoveComponent(x);
                Assert.True(result);

                return;
            }

            Assert.Fail("Unsuccessful get of component!");
        }
    }
}
