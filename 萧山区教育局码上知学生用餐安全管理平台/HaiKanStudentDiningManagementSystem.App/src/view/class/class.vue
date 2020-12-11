<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.class.query.totalCount"
        :pageSize="stores.class.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.class.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchClass()"
                    >
                      <!-- <Select
                        slot="prepend"
                        v-model="stores.class.query.isDeleted"
                        @on-change="handleSearchClass"
                        placeholder="删除状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.class.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select> -->
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <!-- <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button> -->
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'add'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增班级"
                >新增班级</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.class.data"
          :columns="stores.class.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button v-can="'delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip  placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form
        :model="formModel.fields"
        ref="formClass"
        :rules="formModel.rules"
        label-position="left"
      >
        <FormItem label="班级名称" prop="className" label-position="left">
          <Input v-model="formModel.fields.className" placeholder="请输入班级名称" />
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitClass">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
// import {
//     getDepartmentList,
//     createDepartment,
//     loadDepartment,
//     editDepartment,
//     deleteDepartment,
//     batchCommand
// } from "@/api/rbac/class";
import {
  ClassList,
  ClassCreate,
  loadClass,
  ClassEdit,
  deleteClass,
  ClassBatchCommand,
} from "@/api/class/class";
export default {
  name: "class",
  components: {
    DzTable,
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
      },
      formModel: {
        opened: false,
        title: "创建班级",
        mode: "create",
        selection: [],
        fields: {
          className: "",
        },
        rules: {
          className: [
            { type: "string", required: true, message: "请输入班级名", },
          ],
        },
      },
      stores: {
        class: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" },
            ],
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "班级名", key: "className" },

            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          columns1: [
            { type: "selection", width: 50, key: "handle" },
            { title: "班级名", key: "className" },

            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          columns2: [
            { type: "selection", width: 50, key: "handle" },
            { title: "学校", key: "schoolName" },
            { title: "班级名", key: "className" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static",
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建班级";
      }
      if (this.formModel.mode === "edit") {
        return "编辑班级";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.classUuid);
    },
  },
  methods: {
    loadclassList() {
      ClassList(this.stores.class.query).then((res) => {
        this.stores.class.data = res.data.data;
        this.stores.class.query.totalCount = res.data.totalCount;
      });
    },
    handlePageChanged(page) {
      this.stores.class.query.currentPage = page;
      this.loadclassList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.class.query.pageSize = pageSize;
      this.loadclassList();
    },
    handleSortChange(column) {
      this.stores.class.query.sort.direction = column.order;
      this.stores.class.query.sort.field = column.key;
      this.loadclassList();
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      this.handleResetFormClass();
      this.doLoadClass(row.classUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadclassList();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormClass();
    },
    handleSubmitClass() {
      let valid = this.validateClassForm();
      //console.log(valid);
      //console.log(this.formModel.fields);
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateClass();
        }
        if (this.formModel.mode === "edit") {
          this.doEditClass();
        }
      }
    },
    handleResetFormClass() {
      this.$refs["formClass"].resetFields();
    },
    doCreateClass() {
      ClassCreate(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadclassList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditClass() {
      ClassEdit(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadclassList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateClassForm() {
      let _valid = false;
      this.$refs["formClass"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadClass(classUuid) {
      loadClass({ guid: classUuid }).then((res) => {
        this.formModel.fields = res.data.data;
      });
    },
    handleDelete(row) {
      this.doDelete(row.classUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteClass(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadclassList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    doBatchCommand(command) {
      ClassBatchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadclassList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchClass() {
      this.loadclassList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
  },
  mounted() {
    if (this.$store.state.user.schoolguid != null) {
      this.stores.class.columns=this.stores.class.columns1;
    }else{
      this.stores.class.columns=this.stores.class.columns2;
    }

    this.loadclassList();
  },
};
</script>

<style scoped>
</style>
