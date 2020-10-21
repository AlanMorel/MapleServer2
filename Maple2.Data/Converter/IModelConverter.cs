namespace Maple2.Data.Converter {
    public interface IModelConverter<T, TModel> {
        public TModel ToModel(T value, TModel model = default);
        public T FromModel(TModel model);
    }
}