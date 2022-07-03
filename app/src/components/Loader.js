const Loader = ({ loading, children }) => {
  return (
    <>
      <div className={"ui dimmer " + (loading ? "active" : "")}>
        <div className="ui loader"></div>
      </div>
      {children}
    </>
  );
};

export default Loader;
